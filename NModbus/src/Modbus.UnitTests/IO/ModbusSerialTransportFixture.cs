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

namespace Modbus.UnitTests.IO
{
	[TestFixture]
	public class ModbusSerialTransportFixture : ModbusMessageFixture
	{
		[Test, ExpectedException(typeof(SlaveException))]
		public void CreateResponseSlaveException()
		{
			ModbusSerialTransport transport = new ModbusAsciiTransport();
			transport.CreateResponse<ReadCoilsInputsResponse>(new byte[] { 10, 129, 2, 115 });
		}

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
			ReadCoilsInputsResponse expectedResponse = new ReadCoilsInputsResponse(2, 1, new DiscreteCollection(true, false, false, false, false, false, false, true));
			byte lrc = ModbusUtil.CalculateLrc(expectedResponse.MessageFrame);
			ReadCoilsInputsResponse response = transport.CreateResponse<ReadCoilsInputsResponse>(new byte[] { 2, Modbus.ReadCoils, 1, 129, lrc });
			AssertModbusMessagePropertiesAreEqual(expectedResponse, response);
		}
	}
}
