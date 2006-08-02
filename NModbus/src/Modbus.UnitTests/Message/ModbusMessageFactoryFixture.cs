using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Message;
using Modbus.Util;
using Modbus.Data;
using System.Reflection;

namespace Modbus.UnitTests.Message
{
	[TestFixture]
	public class ModbusMessageFactoryFixture : ModbusMessageFixture
	{
		[Test]
		public void CreateModbusMessageReadCoilsRequest()
		{
			byte[] frame = new byte[] { 2, Modbus.ReadCoils, 1, 2, 0, 0, 251 };
			ReadCoilsRequest request = ModbusMessageFactory.CreateModbusMessage<ReadCoilsRequest>(frame);
			AssertModbusMessagePropertiesAreEqual(request, new ReadCoilsRequest(2, 513, 0));
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void CreateModbusMessageReadCoilsRequestWithInvalidFrameSize()
		{
			byte[] frame = new byte[] { 11, Modbus.ReadCoils, 4, 1, 2 };
			ReadCoilsRequest request = ModbusMessageFactory.CreateModbusMessage<ReadCoilsRequest>(frame);
			Assert.Fail();
		}

		[Test]
		public void CreateModbusMessageReadCoilsResponse()
		{
			byte[] frame = new byte[] { 11, Modbus.ReadCoils, 1, 1 };
			ReadCoilsResponse response = ModbusMessageFactory.CreateModbusMessage<ReadCoilsResponse>(frame);
			AssertModbusMessagePropertiesAreEqual(new ReadCoilsResponse(11, 1, new CoilDiscreteCollection(true, false, false, false)), response);
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void CreateModbusMessageReadCoilsResponseWithNoByteCount()
		{
			byte[] frame = new byte[] { 11, Modbus.ReadCoils };
			ReadCoilsResponse response = ModbusMessageFactory.CreateModbusMessage<ReadCoilsResponse>(frame);
			Assert.Fail();
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void CreateModbusMessageReadCoilsResponseWithInvalidDataSize()
		{
			byte[] frame = new byte[] { 11, Modbus.ReadCoils, 4, 1, 2, 3 };
			ReadCoilsResponse response = ModbusMessageFactory.CreateModbusMessage<ReadCoilsResponse>(frame);
			Assert.Fail();
		}

		[Test]
		public void CreateModbusMessageReadHoldingRegistersRequest()
		{
			ReadHoldingRegistersRequest request = ModbusMessageFactory.CreateModbusMessage<ReadHoldingRegistersRequest>(new byte[] { 11, Modbus.ReadHoldingRegisters, 0, 0, 5, 0 });
			AssertModbusMessagePropertiesAreEqual(new ReadHoldingRegistersRequest(11, 0, 5), request);
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void CreateModbusMessageReadHoldingRegistersRequestWithInvalidFrameSize()
		{
			ReadHoldingRegistersRequest request = ModbusMessageFactory.CreateModbusMessage<ReadHoldingRegistersRequest>(new byte[] { 11, Modbus.ReadHoldingRegisters, 0, 0, 5 });
		}

		[Test]
		public void CreateModbusMessageReadHoldingRegistersResponse()
		{
			ReadHoldingRegistersResponse response = ModbusMessageFactory.CreateModbusMessage<ReadHoldingRegistersResponse>(new byte[] { 11, Modbus.ReadHoldingRegisters, 4, 0, 3, 0, 4 });
			AssertModbusMessagePropertiesAreEqual(new ReadHoldingRegistersResponse(11, 4, new HoldingRegisterCollection(3, 4)), response);
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void CreateModbusMessageReadHoldingRegistersResponseWithInvalidFrameSize()
		{
			ModbusMessageFactory.CreateModbusMessage<ReadHoldingRegistersResponse>(new byte[] { 11, Modbus.ReadHoldingRegisters });
		}

		[Test]
		public void CreateModbusMessageSlaveExceptionResponse()
		{
			SlaveExceptionResponse response = ModbusMessageFactory.CreateModbusMessage<SlaveExceptionResponse>(new byte[] { 11, 129, 2 });
			AssertModbusMessagePropertiesAreEqual(new SlaveExceptionResponse(11, Modbus.ReadCoils, 2), response);
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void CreateModbusMessageSlaveExceptionResponseWithInvalidFrameSize()
		{
			ModbusMessageFactory.CreateModbusMessage<SlaveExceptionResponse>(new byte[] { 11, 128 });
			Assert.Fail();
		}

		[Test]
		public void CreateModbusMessageWriteSingleCoilRequestResponse()
		{
			WriteSingleCoilRequestResponse request = ModbusMessageFactory.CreateModbusMessage<WriteSingleCoilRequestResponse>(new byte[] { 11, Modbus.WriteSingleCoil, 0, 105, byte.MaxValue, 0 });
			WriteSingleCoilRequestResponse expectedRequest = new WriteSingleCoilRequestResponse(11, 105, true);
			Assert.AreEqual(expectedRequest.StartAddress, request.StartAddress);
			Assert.AreEqual(expectedRequest.Data[0], request.Data[0]);
			AssertModbusMessagePropertiesAreEqual(expectedRequest, request);
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void CreateModbusMessageWriteSingleCoilRequestResponseWithInvalidFrameSize()
		{
			WriteSingleCoilRequestResponse request = ModbusMessageFactory.CreateModbusMessage<WriteSingleCoilRequestResponse>(new byte[] { 11, Modbus.WriteSingleCoil, 0, 105, byte.MaxValue });
			Assert.Fail();
		}

		[Test]
		public void CreateModbusMessageWriteSingleRegisterRequestResponse()
		{
			WriteSingleRegisterRequestResponse request = ModbusMessageFactory.CreateModbusMessage<WriteSingleRegisterRequestResponse>(new byte[] { 11, Modbus.WriteSingleRegister, 0, 1, 0, 3 });
			WriteSingleRegisterRequestResponse expectedRequest = new WriteSingleRegisterRequestResponse(11, 1, 3);
			Assert.AreEqual(expectedRequest.StartAddress, request.StartAddress);
			Assert.AreEqual(expectedRequest.Data[0], request.Data[0]);
			AssertModbusMessagePropertiesAreEqual(expectedRequest, request);
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void CreateModbusMessageWriteSingleRegisterRequestResponseWithInvalidFrameSize()
		{
			WriteSingleRegisterRequestResponse request = ModbusMessageFactory.CreateModbusMessage<WriteSingleRegisterRequestResponse>(new byte[] { 11, Modbus.WriteSingleRegister, 0, 1, 0 });
			Assert.Fail();
		}
	}
}
