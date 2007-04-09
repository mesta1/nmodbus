using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using Modbus.IO;
using Modbus.Message;
using Modbus.Data;
using Modbus.Util;
using System.IO;

namespace Modbus.UnitTests.IO
{
	[TestFixture]
	public class ModbusTransportFixture
	{
		[Test]
		public void UnicastMessage()
		{
			MockRepository mocks = new MockRepository();
			ModbusTransport transport = mocks.PartialMock<ModbusTransport>();
			transport.Write(null);
			LastCall.IgnoreArguments();
			// read 4 coils from slave id 2
			Expect.Call(transport.ReadResponse()).Return(new byte[] { 2, 1, 1, 5 });

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
			Expect.Call(transport.ReadResponse()).Return(new byte[] { 2, 1, 1, 5 }).Repeat.Times(Modbus.DefaultRetries + 1);

			mocks.ReplayAll();

			ReadCoilsInputsRequest request = new ReadCoilsInputsRequest(Modbus.ReadInputs, 2, 3, 4);
			ReadCoilsInputsResponse response = transport.UnicastMessage<ReadCoilsInputsResponse>(request);

			mocks.VerifyAll();
		}

		[Test, ExpectedException(typeof(SlaveException))]
		public void UnicastMessage_SlaveException()
		{
			MockRepository mocks = new MockRepository();
			ModbusTransport transport = mocks.PartialMock<ModbusTransport>();
			transport.Write(null);
			LastCall.IgnoreArguments().Repeat.Times(Modbus.DefaultRetries + 1);
			// slave exception
			Expect.Call(transport.ReadResponse()).Return(new byte[] { 2, 129, 1, 5 }).Repeat.Times(Modbus.DefaultRetries + 1);

			mocks.ReplayAll();

			ReadCoilsInputsRequest request = new ReadCoilsInputsRequest(Modbus.ReadInputs, 2, 3, 4);
			ReadCoilsInputsResponse response = transport.UnicastMessage<ReadCoilsInputsResponse>(request);

			mocks.VerifyAll();
		}

		public delegate byte[] ThrowTimeoutExceptionDelegate();

		[Test, ExpectedException(typeof(TimeoutException))]
		public void UnicastMessage_TimeoutException()
		{
			MockRepository mocks = new MockRepository();
			ModbusTransport transport = mocks.PartialMock<ModbusTransport>();
			transport.Write(null);
			LastCall.IgnoreArguments().Repeat.Times(Modbus.DefaultRetries + 1);
			Expect.Call(transport.ReadResponse())
				.Do((ThrowTimeoutExceptionDelegate) delegate { throw new TimeoutException(); })
				.Repeat.Times(Modbus.DefaultRetries + 1);

			mocks.ReplayAll();

			ReadCoilsInputsRequest request = new ReadCoilsInputsRequest(Modbus.ReadInputs, 2, 3, 4);
			ReadCoilsInputsResponse response = transport.UnicastMessage<ReadCoilsInputsResponse>(request);

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
			Expect.Call(transport.ReadResponse())
				.Do((ThrowTimeoutExceptionDelegate) delegate { throw new TimeoutException(); })
				.Repeat.Times(transport.Retries + 1);

			mocks.ReplayAll();

			ReadCoilsInputsRequest request = new ReadCoilsInputsRequest(Modbus.ReadInputs, 2, 3, 4);
			ReadCoilsInputsResponse response = transport.UnicastMessage<ReadCoilsInputsResponse>(request);

			mocks.VerifyAll();
		}
	}
}
