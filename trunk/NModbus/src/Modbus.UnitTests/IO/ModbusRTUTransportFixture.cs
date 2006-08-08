using System;
using System.Collections.Generic;
using System.Text;
using Modbus.IO;
using Modbus.Message;
using NUnit.Framework;
using System.IO.Ports;

namespace Modbus.UnitTests.IO
{
	[TestFixture]
	public class ModbusRTUTransportFixture
	{
		[Test]
		public void CheckBuildMessageFrame()
		{
			byte[] message = new byte[] { 17, Modbus.ReadCoils, 0, 19, 0, 37, 14, 132 };
			ReadCoilsInputsRequest request = new ReadCoilsInputsRequest(Modbus.ReadCoils, 17, 19, 37);
			Assert.AreEqual(message, new ModbusRTUTransport(new SerialPort()).BuildMessageFrame(request));
		}
	}
}
