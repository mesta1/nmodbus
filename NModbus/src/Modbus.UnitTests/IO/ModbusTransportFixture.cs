using System;
using System.IO;
using Modbus.Data;
using Modbus.IO;
using Modbus.Message;
using NUnit.Framework;
using Rhino.Mocks;
using Modbus.Util;

namespace Modbus.UnitTests.IO
{
	[TestFixture]
	public class ModbusTransportFixture
	{
		delegate ReadCoilsInputsResponse ThrowExceptionDelegate();

		[Test]
		public void UnicastMessage()
		{
			MockRepository mocks = new MockRepository();
			ModbusTransport transport = mocks.PartialMock<ModbusTransport>();
			transport.Write(null);
			LastCall.IgnoreArguments();
			// read 4 coils from slave id 2
			Expect.Call(transport.ReadResponse<ReadCoilsInputsResponse>())
				.Return(new ReadCoilsInputsResponse(Modbus.ReadCoils, 2, 1, new DiscreteCollection(true, false, true, false, false, false, false, false)));

			mocks.ReplayAll();

			ReadCoilsInputsRequest request = new ReadCoilsInputsRequest(Modbus.ReadCoils, 2, 3, 4);
			ReadCoilsInputsResponse expectedResponse = new ReadCoilsInputsResponse(Modbus.ReadCoils, 2, 1, new DiscreteCollection(true, false, true, false, false, false, false, false));
			ReadCoilsInputsResponse response = transport.UnicastMessage<ReadCoilsInputsResponse>(request);
			Assert.AreEqual(expectedResponse.MessageFrame, response.MessageFrame);

			mocks.VerifyAll();
		}

		[Test, ExpectedException(typeof(IOException))]
		public void UnicastMessage_WrongResponseFunctionCode()
		{
			MockRepository mocks = new MockRepository();
			ModbusTransport transport = mocks.PartialMock<ModbusTransport>();
			transport.Write(null);
			LastCall.IgnoreArguments().Repeat.Times(Modbus.DefaultRetries + 1);
			// read 4 coils from slave id 2
			Expect.Call(transport.ReadResponse<ReadCoilsInputsResponse>())
				.Return(new ReadCoilsInputsResponse(Modbus.ReadCoils, 2, 0, new DiscreteCollection()))
				.Repeat.Times(Modbus.DefaultRetries + 1);

			mocks.ReplayAll();

			ReadCoilsInputsRequest request = new ReadCoilsInputsRequest(Modbus.ReadInputs, 2, 3, 4);
			transport.UnicastMessage<ReadCoilsInputsResponse>(request);

			mocks.VerifyAll();
		}

		[Test, ExpectedException(typeof(SlaveException))]
		public void UnicastMessage_SlaveException()
		{
			MockRepository mocks = new MockRepository();
			ModbusTransport transport = mocks.PartialMock<ModbusTransport>();
			transport.Write(null);
			LastCall.IgnoreArguments().Repeat.Times(Modbus.DefaultRetries + 1);
			Expect.Call(transport.ReadResponse<ReadCoilsInputsResponse>())
				.Do((ThrowExceptionDelegate) delegate { throw new SlaveException(); })
				.Repeat.Times(Modbus.DefaultRetries + 1);

			mocks.ReplayAll();

			ReadCoilsInputsRequest request = new ReadCoilsInputsRequest(Modbus.ReadInputs, 2, 3, 4);
			transport.UnicastMessage<ReadCoilsInputsResponse>(request);

			mocks.VerifyAll();
		}

		[Test]
		public void UnicastMessage_AcknowlegeSlaveException()
		{
			MockRepository mocks = new MockRepository();
			ModbusTransport transport = mocks.PartialMock<ModbusTransport>();
			transport.Write(null);
			LastCall.IgnoreArguments();

			Expect.Call(transport.ReadResponse<ReadHoldingInputRegistersResponse>())
				.Return(new SlaveExceptionResponse(1, Modbus.ReadHoldingRegisters + Modbus.ExceptionOffset, Modbus.Acknowlege))
				.Repeat.Times(8);

			Expect.Call(transport.ReadResponse<ReadHoldingInputRegistersResponse>())
				.Return(new ReadHoldingInputRegistersResponse(Modbus.ReadHoldingRegisters, 1, 1, new RegisterCollection(1)));

			mocks.ReplayAll();

			ReadHoldingInputRegistersRequest request = new ReadHoldingInputRegistersRequest(Modbus.ReadHoldingRegisters, 1, 1, 1);
			ReadHoldingInputRegistersResponse expectedResponse = new ReadHoldingInputRegistersResponse(Modbus.ReadHoldingRegisters, 1, 1, new RegisterCollection(1));
			ReadHoldingInputRegistersResponse response = transport.UnicastMessage<ReadHoldingInputRegistersResponse>(request);
			Assert.AreEqual(expectedResponse.MessageFrame, response.MessageFrame);

			mocks.VerifyAll();
		}

		[Test, ExpectedException(typeof(TimeoutException))]
		public void UnicastMessage_TimeoutException()
		{
			MockRepository mocks = new MockRepository();
			ModbusTransport transport = mocks.PartialMock<ModbusTransport>();
			transport.Write(null);
			LastCall.IgnoreArguments().Repeat.Times(Modbus.DefaultRetries + 1);
			Expect.Call(transport.ReadResponse<ReadCoilsInputsResponse>())
				.Do((ThrowExceptionDelegate) delegate { throw new TimeoutException(); })
				.Repeat.Times(Modbus.DefaultRetries + 1);

			mocks.ReplayAll();

			ReadCoilsInputsRequest request = new ReadCoilsInputsRequest(Modbus.ReadInputs, 2, 3, 4);
			transport.UnicastMessage<ReadCoilsInputsResponse>(request);

			mocks.VerifyAll();
		}

		[Test, ExpectedException(typeof(TimeoutException))]
		public void UnicastMessage_Retries()
		{
			MockRepository mocks = new MockRepository();
			ModbusTransport transport = mocks.PartialMock<ModbusTransport>();
			transport.Retries = 5;
			transport.Write(null);
			LastCall.IgnoreArguments().Repeat.Times(transport.Retries + 1);
			Expect.Call(transport.ReadResponse<ReadCoilsInputsResponse>())
				.Do((ThrowExceptionDelegate) delegate { throw new TimeoutException(); })
				.Repeat.Times(transport.Retries + 1);

			mocks.ReplayAll();

			ReadCoilsInputsRequest request = new ReadCoilsInputsRequest(Modbus.ReadInputs, 2, 3, 4);
			transport.UnicastMessage<ReadCoilsInputsResponse>(request);

			mocks.VerifyAll();
		}

		[Test]
		public void CreateResponse_SlaveException()
		{
			ModbusTransport transport = new ModbusAsciiTransport();
			byte[] frame = { 2, 129, 2 };
			IModbusMessage message = transport.CreateResponse<ReadCoilsInputsResponse>(CollectionUtil.Combine(frame, new byte[] { ModbusUtil.CalculateLrc(frame) }));
			Assert.IsTrue(message is SlaveExceptionResponse);
		}

		[Test, ExpectedException(typeof(IOException))]
		public void ValidateResponse_MismatchingFunctionCodes()
		{
			MockRepository mocks = new MockRepository();
			ModbusTransport transport = mocks.PartialMock<ModbusTransport>();

			IModbusMessage request = new ReadCoilsInputsRequest(Modbus.ReadCoils, 1, 1, 1);
			IModbusMessage response = new ReadHoldingInputRegistersResponse(Modbus.ReadHoldingRegisters, 1, 1, null);

			mocks.ReplayAll();
			transport.ValidateResponse(request, response);
			mocks.VerifyAll();
		}

		[Test]
		public void ValidateResponse()
		{
			MockRepository mocks = new MockRepository();
			ModbusTransport transport = mocks.PartialMock<ModbusTransport>();

			IModbusMessage request = new ReadCoilsInputsRequest(Modbus.ReadCoils, 1, 1, 1);
			IModbusMessage response = new ReadCoilsInputsResponse(Modbus.ReadCoils, 1, 1, null);

			mocks.ReplayAll();
			transport.ValidateResponse(request, response);
			mocks.VerifyAll();
		}
	}
}
