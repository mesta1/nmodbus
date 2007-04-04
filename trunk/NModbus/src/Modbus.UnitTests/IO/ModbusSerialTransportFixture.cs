using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.IO;
using Modbus.Message;
using Modbus.Data;
using Modbus.UnitTests.Message;
using Modbus.Util;
using System.IO;
using System.IO.Ports;
using Rhino.Mocks;

namespace Modbus.UnitTests.IO
{
	[TestFixture]
	public class ModbusSerialTransportFixture : ModbusMessageFixture
	{
		[Test, ExpectedException(typeof(IOException))]
		public void CreateResponseErroneousLrc()
		{
			ModbusAsciiTransport transport = new ModbusAsciiTransport();
			transport.CreateResponse<ReadCoilsInputsResponse>(new byte[] { 19, Modbus.ReadCoils, 0, 0, 0, 2, 115 });
			Assert.Fail();
		}

		[Test]
		public void CreateResponse()
		{
			ModbusAsciiTransport transport = new ModbusAsciiTransport();
			ReadCoilsInputsResponse expectedResponse = new ReadCoilsInputsResponse(Modbus.ReadCoils, 2, 1, new DiscreteCollection(true, false, false, false, false, false, false, true));
			byte lrc = ModbusUtil.CalculateLrc(expectedResponse.MessageFrame);
			ReadCoilsInputsResponse response = transport.CreateResponse<ReadCoilsInputsResponse>(new byte[] { 2, Modbus.ReadCoils, 1, 129, lrc });
			AssertModbusMessagePropertiesAreEqual(expectedResponse, response);
		}

		[Test]
		public void SetupTimeoutsNonDefaultTimeout()
		{			
			int nonDefaultReadTimeout = 67;
			int nonDefaultWriteTimeout = 42;
			MockRepository mocks = new MockRepository();
			SerialPortStreamAdapter mockStream = mocks.CreateMock<SerialPortStreamAdapter>();

			Expect.Call(mockStream.WriteTimeout).Return(nonDefaultWriteTimeout);
			Expect.Call(mockStream.WriteTimeout).Return(nonDefaultWriteTimeout);
			mockStream.WriteTimeout = nonDefaultWriteTimeout;			
			Expect.Call(mockStream.ReadTimeout).Return(nonDefaultReadTimeout);
			Expect.Call(mockStream.ReadTimeout).Return(nonDefaultReadTimeout);
			mockStream.ReadTimeout =  nonDefaultReadTimeout;

			mocks.ReplayAll();
			ModbusAsciiTransport transport = new ModbusAsciiTransport(mockStream);
			mocks.VerifyAll();
		}

		[Test]
		public void SetupTimeoutsDefaultTimeout()
		{
			MockRepository mocks = new MockRepository();
			SerialPortStreamAdapter mockStream = mocks.CreateMock<SerialPortStreamAdapter>();

			Expect.Call(mockStream.WriteTimeout).Return(SerialPort.InfiniteTimeout);
			mockStream.WriteTimeout = Modbus.DefaultTimeout;
			Expect.Call(mockStream.ReadTimeout).Return(SerialPort.InfiniteTimeout);
			mockStream.ReadTimeout = Modbus.DefaultTimeout;

			mocks.ReplayAll();
			ModbusAsciiTransport transport = new ModbusAsciiTransport(mockStream);
			mocks.VerifyAll();
		}
	}
}
