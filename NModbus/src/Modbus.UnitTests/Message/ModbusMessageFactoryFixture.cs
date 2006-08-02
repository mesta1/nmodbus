using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Message;
using Modbus.Util;
using Modbus.Data;
using System.Reflection;

using WriteRequest = Modbus.Message.WriteSingleRegisterRequest;
using WriteResponse = Modbus.Message.WriteSingleRegisterRequest;

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
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void CreateModbusMessageReadCoilsResponseWithInvalidDataSize()
		{
			byte[] frame = new byte[] { 11, Modbus.ReadCoils, 4, 1, 2, 3 };
			ReadCoilsResponse response = ModbusMessageFactory.CreateModbusMessage<ReadCoilsResponse>(frame);
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
		}

		[Test]
		public void CreateModbusMessageWriteSingleCoilResponse()
		{
			WriteSingleCoilResponse response = ModbusMessageFactory.CreateModbusMessage<WriteSingleCoilResponse>(new byte[] { 2, Modbus.WriteSingleCoil, 0, 105, byte.MaxValue, 0, 91 });
			AssertModbusMessagePropertiesAreEqual(new WriteSingleCoilRequest(2, 105, true), response);
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void CreateModbusMessageWriteSingleCoilResponseWithInvalidFrameSize()
		{
			WriteSingleCoilResponse response = ModbusMessageFactory.CreateModbusMessage<WriteSingleCoilResponse>(new byte[] { 11, Modbus.WriteSingleCoil, 0, 105, byte.MaxValue });
		}

		[Test]
		public void CreateModbusMessageWriteSingleCoilRequest()
		{
			WriteSingleCoilRequest request = ModbusMessageFactory.CreateModbusMessage<WriteSingleCoilRequest>(new byte[] { 11, Modbus.WriteSingleCoil, 0, 105, byte.MaxValue, 0 });
			WriteSingleCoilRequest expectedRequest = new WriteSingleCoilRequest(11, 105, true);
			Assert.AreEqual(expectedRequest.StartAddress, request.StartAddress);
			Assert.AreEqual(expectedRequest.Data[0], request.Data[0]);
			AssertModbusMessagePropertiesAreEqual(expectedRequest, request);
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void CreateModbusMessageWriteSingleCoilRequestWithInvalidFrameSize()
		{
			WriteSingleCoilRequest request = ModbusMessageFactory.CreateModbusMessage<WriteSingleCoilRequest>(new byte[] { 11, Modbus.WriteSingleCoil, 0, 105, byte.MaxValue });
		}

		[Test]
		public void CreateModbusMessageWriteSingleRegisterRequest()
		{
			WriteSingleRegisterRequest request = ModbusMessageFactory.CreateModbusMessage<WriteSingleRegisterRequest>(new byte[] { 11, Modbus.WriteSingleRegister, 0, 1, 0, 3 });
			WriteSingleRegisterRequest expectedRequest = new WriteSingleRegisterRequest(11, 1, 3);
			Assert.AreEqual(expectedRequest.StartAddress, request.StartAddress);
			Assert.AreEqual(expectedRequest.Data[0], request.Data[0]);
			AssertModbusMessagePropertiesAreEqual(expectedRequest, request);
		}

		[Test]
		public void CreateModbusMessageWriteSingleRegisterResponse()
		{
			WriteSingleRegisterResponse request = ModbusMessageFactory.CreateModbusMessage<WriteSingleRegisterResponse>(new byte[] { 11, Modbus.WriteSingleRegister, 0, 1, 0, 3 });
			WriteSingleRegisterResponse expectedRequest = new WriteSingleRegisterResponse(11, 1, 3);
			Assert.AreEqual(expectedRequest.StartAddress, request.StartAddress);
			Assert.AreEqual(expectedRequest.Data[0], request.Data[0]);
			AssertModbusMessagePropertiesAreEqual(expectedRequest, request);
		}
	}
}
