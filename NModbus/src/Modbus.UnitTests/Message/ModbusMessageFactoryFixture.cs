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
			ReadCoilsInputsRequest request = ModbusMessageFactory.CreateModbusMessage<ReadCoilsInputsRequest>(new byte[] { 11, Modbus.ReadCoils, 0, 19, 0, 37 });
			ReadCoilsInputsRequest expectedRequest = new ReadCoilsInputsRequest(Modbus.ReadCoils, 11, 19,  37);
			AssertModbusMessagePropertiesAreEqual(request, expectedRequest);
			Assert.AreEqual(expectedRequest.StartAddress, request.StartAddress);
			Assert.AreEqual(expectedRequest.NumberOfPoints, request.NumberOfPoints);
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void CreateModbusMessageReadCoilsRequestWithInvalidFrameSize()
		{
			byte[] frame = new byte[] { 11, Modbus.ReadCoils, 4, 1, 2 };
			ReadCoilsInputsRequest request = ModbusMessageFactory.CreateModbusMessage<ReadCoilsInputsRequest>(frame);
			Assert.Fail();
		}

		[Test]
		public void CreateModbusMessageReadCoilsResponse()
		{
			ReadCoilsResponse response = ModbusMessageFactory.CreateModbusMessage<ReadCoilsResponse>(new byte[] { 11, Modbus.ReadCoils, 1, 1 });
			ReadCoilsResponse expectedResponse = new ReadCoilsResponse(11, 1, new DiscreteCollection(true, false, false, false));
			AssertModbusMessagePropertiesAreEqual(expectedResponse, response);
			Assert.AreEqual(expectedResponse.Data.NetworkBytes, response.Data.NetworkBytes);
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
			ReadHoldingInputRegistersRequest request = ModbusMessageFactory.CreateModbusMessage<ReadHoldingInputRegistersRequest>(new byte[] { 17, Modbus.ReadHoldingRegisters, 0, 107, 0, 3 });
			ReadHoldingInputRegistersRequest expectedRequest = new ReadHoldingInputRegistersRequest(Modbus.ReadHoldingRegisters, 17, 107, 3);
			AssertModbusMessagePropertiesAreEqual(expectedRequest, request);
			Assert.AreEqual(expectedRequest.StartAddress, request.StartAddress);
			Assert.AreEqual(expectedRequest.NumberOfPoints, request.NumberOfPoints);
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void CreateModbusMessageReadHoldingRegistersRequestWithInvalidFrameSize()
		{
			ReadHoldingInputRegistersRequest request = ModbusMessageFactory.CreateModbusMessage<ReadHoldingInputRegistersRequest>(new byte[] { 11, Modbus.ReadHoldingRegisters, 0, 0, 5 });
		}

		[Test]
		public void CreateModbusMessageReadHoldingRegistersResponse()
		{
			ReadHoldingRegistersResponse response = ModbusMessageFactory.CreateModbusMessage<ReadHoldingRegistersResponse>(new byte[] { 11, Modbus.ReadHoldingRegisters, 4, 0, 3, 0, 4 });
			ReadHoldingRegistersResponse expectedResponse = new ReadHoldingRegistersResponse(11, 4, new RegisterCollection(3, 4));
			AssertModbusMessagePropertiesAreEqual(expectedResponse, response);
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
		public void CreateModbusMessageSlaveExceptionResponseWithInvalidFunctionCode()
		{
			SlaveExceptionResponse response = ModbusMessageFactory.CreateModbusMessage<SlaveExceptionResponse>(new byte[] { 11, 128, 2 });
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
			WriteSingleCoilRequestResponse request = ModbusMessageFactory.CreateModbusMessage<WriteSingleCoilRequestResponse>(new byte[] { 17, Modbus.WriteSingleCoil, 0, 172, byte.MaxValue, 0 });
			WriteSingleCoilRequestResponse expectedRequest = new WriteSingleCoilRequestResponse(17, 172, true);
			AssertModbusMessagePropertiesAreEqual(expectedRequest, request);
			Assert.AreEqual(expectedRequest.StartAddress, request.StartAddress);
			Assert.AreEqual(expectedRequest.Data.NetworkBytes, request.Data.NetworkBytes);			
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
			WriteSingleRegisterRequestResponse request = ModbusMessageFactory.CreateModbusMessage<WriteSingleRegisterRequestResponse>(new byte[] { 17, Modbus.WriteSingleRegister, 0, 1, 0, 3 });
			WriteSingleRegisterRequestResponse expectedRequest = new WriteSingleRegisterRequestResponse(17, 1, 3);
			AssertModbusMessagePropertiesAreEqual(expectedRequest, request);
			Assert.AreEqual(expectedRequest.StartAddress, request.StartAddress);
			Assert.AreEqual(expectedRequest.Data.NetworkBytes, request.Data.NetworkBytes);
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void CreateModbusMessageWriteSingleRegisterRequestResponseWithInvalidFrameSize()
		{
			WriteSingleRegisterRequestResponse request = ModbusMessageFactory.CreateModbusMessage<WriteSingleRegisterRequestResponse>(new byte[] { 11, Modbus.WriteSingleRegister, 0, 1, 0 });
			Assert.Fail();
		}

		[Test]
		public void CreateModbusMessageWriteMultipleRegistersRequest()
		{
			WriteMultipleRegistersRequest request = ModbusMessageFactory.CreateModbusMessage<WriteMultipleRegistersRequest>(new byte[] { 11, Modbus.WriteMultipleRegisters, 0, 5, 0, 1, 2, 255, 255 });
			WriteMultipleRegistersRequest expectedRequest = new WriteMultipleRegistersRequest(11, 5, new RegisterCollection(ushort.MaxValue));
			AssertModbusMessagePropertiesAreEqual(expectedRequest, request);		
			Assert.AreEqual(expectedRequest.StartAddress, request.StartAddress);
			Assert.AreEqual(expectedRequest.NumberOfPoints, request.NumberOfPoints);
			Assert.AreEqual(expectedRequest.ByteCount, request.ByteCount);
			Assert.AreEqual(expectedRequest.Data.NetworkBytes, request.Data.NetworkBytes);
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void CreateModbusMessageWriteMultipleRegistersRequestWithInvalidFrameSize()
		{
			WriteMultipleRegistersRequest request = ModbusMessageFactory.CreateModbusMessage<WriteMultipleRegistersRequest>(new byte[] { 11, Modbus.WriteMultipleRegisters, 0, 5, 0, 1, 2 });
			Assert.Fail();
		}

		[Test]
		public void CreateModbusMessageWriteMultipleRegistersResponse()
		{
			WriteMultipleRegistersResponse response = ModbusMessageFactory.CreateModbusMessage<WriteMultipleRegistersResponse>(new byte[] { 17, Modbus.WriteMultipleRegisters, 0, 1, 0, 2 });
			WriteMultipleRegistersResponse expectedResponse = new WriteMultipleRegistersResponse(17, 1, 2);
			AssertModbusMessagePropertiesAreEqual(expectedResponse, response);
			Assert.AreEqual(expectedResponse.StartAddress, response.StartAddress);
			Assert.AreEqual(expectedResponse.NumberOfPoints, response.NumberOfPoints);
		}

		[Test]
		public void CreateModbusMessageWriteMultipleCoilsRequest()
		{
			WriteMultipleCoilsRequest request = ModbusMessageFactory.CreateModbusMessage<WriteMultipleCoilsRequest>(new byte[] { 17, Modbus.WriteMultipleCoils, 0, 19, 0, 10, 2, 205, 1 });
			WriteMultipleCoilsRequest expectedRequest = new WriteMultipleCoilsRequest(17, 19, new DiscreteCollection(true, false, true, true, false, false, true, true, true, false));
			AssertModbusMessagePropertiesAreEqual(expectedRequest, request);
			Assert.AreEqual(expectedRequest.StartAddress, request.StartAddress);
			Assert.AreEqual(expectedRequest.NumberOfPoints, request.NumberOfPoints);
			Assert.AreEqual(expectedRequest.ByteCount, request.ByteCount);
			Assert.AreEqual(expectedRequest.Data.NetworkBytes, request.Data.NetworkBytes);
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void CreateModbusMessageWriteMultipleCoilsRequestWithInvalidFrameSize()
		{
			WriteMultipleCoilsRequest request = ModbusMessageFactory.CreateModbusMessage<WriteMultipleCoilsRequest>(new byte[] { 17, Modbus.WriteMultipleCoils, 0, 19, 0, 10, 2 });
			Assert.Fail();
		}

		[Test]
		public void CreateModbusMessageWriteMultipleCoilsResponse()
		{
			WriteMultipleCoilsResponse response = ModbusMessageFactory.CreateModbusMessage<WriteMultipleCoilsResponse>(new byte[] { 17, Modbus.WriteMultipleCoils, 0, 19, 0, 10 });
			WriteMultipleCoilsResponse expectedResponse = new WriteMultipleCoilsResponse(17, 19, 10);
			AssertModbusMessagePropertiesAreEqual(expectedResponse, response);
			Assert.AreEqual(expectedResponse.StartAddress, response.StartAddress);
			Assert.AreEqual(expectedResponse.NumberOfPoints, response.NumberOfPoints);
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void CreateModbusMessageWriteMultipleCoilsResponseWithInvalidFrameSize()
		{
			WriteMultipleCoilsResponse request = ModbusMessageFactory.CreateModbusMessage<WriteMultipleCoilsResponse>(new byte[] { 17, Modbus.WriteMultipleCoils, 0, 19, 0 });
			Assert.Fail();
		}

		[Test, ExpectedException(typeof(FormatException))]
		public void CreateModbusRequestWithInvalidMessageFrame()
		{
			ModbusMessageFactory.CreateModbusRequest(new byte[] { 0, 1 });
			Assert.Fail();
		}

		[Test, ExpectedException(typeof(ArgumentException))]
		public void CreateModbusRequestWithInvalidFunctionCode()
		{
			ModbusMessageFactory.CreateModbusRequest(new byte[] { 1, 99, 0, 0, 0, 1, 23 });
			Assert.Fail();
		}

		[Test]
		public void CreateModbusRequestForReadCoils()
		{
			ReadCoilsInputsRequest req = new ReadCoilsInputsRequest(1, 2, 1, 10);
			IModbusMessage request = ModbusMessageFactory.CreateModbusRequest(req.MessageFrame);
			Assert.AreEqual(typeof(ReadCoilsInputsRequest), request.GetType());
		}
	}
}
