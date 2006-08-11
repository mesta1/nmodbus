using System;
using System.Collections.Generic;
using System.Text;
using Modbus.IO;
using Modbus.Message;
using NUnit.Framework;
using System.IO.Ports;
using Modbus.Util;

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

		[Test]
		public void NumberOfBytesToReadReadCoils()
		{
			byte[] frame = new byte[] { 0x11, 0x01, 0x05, 0xCD, 0x6B, 0xB2, 0x0E, 0x1B };
			Assert.AreEqual(6, ModbusRTUTransport.NumberOfBytesToRead(frame[1], frame[2], frame[3]));

		}

		[Test]
		public void NumberOfBytesToReadReadCoilsNoData()
		{
			byte[] frame = new byte[] { 0x11, 0x01, 0x00, 0, 0 };
			Assert.AreEqual(1, ModbusRTUTransport.NumberOfBytesToRead(frame[1], frame[2], frame[3]));
		}

		[Test]
		public void NumberOfBytesToReadWriteCoilsResponse()
		{
			byte[] frame = new byte[] { 0x11, 0x0F, 0x00, 0x13, 0x00, 0x0A, 0, 0 };
			Assert.AreEqual(4, ModbusRTUTransport.NumberOfBytesToRead(frame[1], frame[2], frame[3]));
		}
	}
}
