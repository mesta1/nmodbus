using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.IO;
using Modbus.Message;
using System.IO.Ports;

namespace Modbus.UnitTests.IO
{
	[TestFixture]
	public class ModbusASCIITransportFixture
	{
		[Test]
		public void CreateMessageFrame()
		{
			byte[] message = new byte[] { 58, 48, 50, 48, 49, 48, 48, 48, 48, 48, 48, 48, 49, 70, 67, 13, 10 };
			ReadCoilsInputsRequest request = new ReadCoilsInputsRequest(Modbus.ReadCoils, 2, 0, 1);
			Assert.AreEqual(message, new ModbusASCIITransport(new SerialPort()).CreateMessageFrame(request));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ModbusASCIITranpsortNullSerialPort()
		{
			ModbusASCIITransport transport = new ModbusASCIITransport(null);
			Assert.Fail();
		}
	}
}
