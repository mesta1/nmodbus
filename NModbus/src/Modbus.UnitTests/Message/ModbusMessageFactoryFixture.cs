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
		public void CheckCreateModbusMessageReadCoilsRequest()
		{
			byte[] frame = new byte[] { 2, Modbus.ReadCoils, 1, 2, 0, 0, 251 };
			ReadCoilsRequest request = ModbusMessageFactory.CreateModbusMessage<ReadCoilsRequest>(frame);
			AssertModbusMessagePropertiesAreEqual(request, new ReadCoilsRequest(2, 513, 0));
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void CheckCreateModbusMessageReadCoilsRequestWithInvalidFrameSize()
		{
			byte[] frame = new byte[] { 11, Modbus.ReadCoils, 4, 1, 2 };
			ReadCoilsRequest request = ModbusMessageFactory.CreateModbusMessage<ReadCoilsRequest>(frame);
		}

		[Test]
		public void CheckCreateModbusMessageReadCoilsResponse()
		{
			byte[] frame = new byte[] { 11, Modbus.ReadCoils, 1, 1 };
			ReadCoilsResponse response = ModbusMessageFactory.CreateModbusMessage<ReadCoilsResponse>(frame);
			AssertModbusMessagePropertiesAreEqual(new ReadCoilsResponse(11, 1, new CoilDiscreteCollection(true, false, false, false)), response);
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void CheckCreateModbusMessageReadCoilsResponseWithNoByteCount()
		{
			byte[] frame = new byte[] { 11, Modbus.ReadCoils };
			ReadCoilsResponse response = ModbusMessageFactory.CreateModbusMessage<ReadCoilsResponse>(frame);
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void CheckCreateModbusMessageReadCoilsResponseWithInvalidDataSize()
		{
			byte[] frame = new byte[] { 11, Modbus.ReadCoils, 4, 1, 2, 3 };
			ReadCoilsResponse response = ModbusMessageFactory.CreateModbusMessage<ReadCoilsResponse>(frame);
		}

		[Test]
		public void CheckCreateModbusMessageReadHoldingRegistersRequest()
		{
			ReadHoldingRegistersRequest request = ModbusMessageFactory.CreateModbusMessage<ReadHoldingRegistersRequest>(new byte[] { 11, Modbus.ReadHoldingRegisters, 0, 0, 5, 0 });
			AssertModbusMessagePropertiesAreEqual(new ReadHoldingRegistersRequest(11, 0, 5), request);
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void CheckCreateModbusMessageReadHoldingRegistersRequestWithInvalidFrameSize()
		{
			ReadHoldingRegistersRequest request = ModbusMessageFactory.CreateModbusMessage<ReadHoldingRegistersRequest>(new byte[] { 11, Modbus.ReadHoldingRegisters, 0, 0, 5 });
		}

		[Test]
		public void CheckCreateModbusMessageReadHoldingRegistersResponse()
		{
			ReadHoldingRegistersResponse response = ModbusMessageFactory.CreateModbusMessage<ReadHoldingRegistersResponse>(new byte[] { 11, Modbus.ReadHoldingRegisters, 4, 0, 3, 0, 4 });
			AssertModbusMessagePropertiesAreEqual(new ReadHoldingRegistersResponse(11, 4, new HoldingRegisterCollection(3, 4)), response);
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void CheckCreateModbusMessageReadHoldingRegistersResponseWithInvalidFrameSize()
		{
			ModbusMessageFactory.CreateModbusMessage<ReadHoldingRegistersResponse>(new byte[] { 11, Modbus.ReadHoldingRegisters });
		}

		[Test]
		public void CheckCreateModbusMessageSlaveExceptionResponse()
		{
			SlaveExceptionResponse response = ModbusMessageFactory.CreateModbusMessage<SlaveExceptionResponse>(new byte[] { 11, 129, 2 });
			AssertModbusMessagePropertiesAreEqual(new SlaveExceptionResponse(11, Modbus.ReadCoils, 2), response);
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void CheckCreateModbusMessageSlaveExceptionResponseWithInvalidFrameSize()
		{
			ModbusMessageFactory.CreateModbusMessage<SlaveExceptionResponse>(new byte[] { 11, 128 });
		}
	}
}
