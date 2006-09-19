using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.IO;
using Modbus.Message;

namespace Modbus.UnitTests.IO
{
	[TestFixture]
	public class ModbusAsciiTransportFixture
	{
		[Test]
		public void BuildMessageFrame()
		{
			byte[] message = new byte[] { 58, 48, 50, 48, 49, 48, 48, 48, 48, 48, 48, 48, 49, 70, 67, 13, 10 };
			ReadCoilsInputsRequest request = new ReadCoilsInputsRequest(Modbus.ReadCoils, 2, 0, 1);
			Assert.AreEqual(message, new ModbusAsciiTransport().BuildMessageFrame(request));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ModbusASCIITranpsortNullSerialPort()
		{
			ModbusAsciiTransport transport = new ModbusAsciiTransport(null);
			Assert.Fail();
		}
	}
}
