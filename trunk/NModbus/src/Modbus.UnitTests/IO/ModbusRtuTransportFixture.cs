using System;
using System.Collections.Generic;
using System.Text;
using Modbus.IO;
using Modbus.Message;
using NUnit.Framework;
using Modbus.Util;
using System.IO;
using Rhino.Mocks;
using System.Net.Sockets;

namespace Modbus.UnitTests.IO
{
	[TestFixture]
	public class ModbusRtuTransportFixture
	{
		[Test]
		public void BuildMessageFrame()
		{
			byte[] message = new byte[] { 17, Modbus.ReadCoils, 0, 19, 0, 37, 14, 132 };
			ReadCoilsInputsRequest request = new ReadCoilsInputsRequest(Modbus.ReadCoils, 17, 19, 37);
			Assert.AreEqual(message, new ModbusRtuTransport().BuildMessageFrame(request));
		}

		[Test]
		public void NumberOfBytesToReadReadCoils()
		{
			byte[] frame = new byte[] { 0x11, 0x01, 0x05, 0xCD, 0x6B, 0xB2, 0x0E, 0x1B };
			Assert.AreEqual(6, ModbusRtuTransport.NumberOfBytesToRead(frame[1], frame[2], frame[3]));

		}

		[Test]
		public void NumberOfBytesToReadReadCoilsNoData()
		{
			byte[] frame = new byte[] { 0x11, 0x01, 0x00, 0, 0 };
			Assert.AreEqual(1, ModbusRtuTransport.NumberOfBytesToRead(frame[1], frame[2], frame[3]));
		}

		[Test]
		public void NumberOfBytesToReadWriteCoilsResponse()
		{
			byte[] frame = new byte[] { 0x11, 0x0F, 0x00, 0x13, 0x00, 0x0A, 0, 0 };
			Assert.AreEqual(4, ModbusRtuTransport.NumberOfBytesToRead(frame[1], frame[2], frame[3]));
		}

		[Test]
		public void ChecksumsMatchSucceed()
		{
			ModbusRtuTransport transport = new ModbusRtuTransport();
			ReadCoilsInputsRequest message = new ReadCoilsInputsRequest(Modbus.ReadCoils, 17, 19, 37);
			byte[] frame = new byte[] { 17, Modbus.ReadCoils, 0, 19, 0, 37, 14, 132};
			Assert.IsTrue(transport.ChecksumsMatch(message, frame));
		}

		[Test]
		public void ChecksumsMatchFail()
		{
			ModbusRtuTransport transport = new ModbusRtuTransport();
			ReadCoilsInputsRequest message = new ReadCoilsInputsRequest(Modbus.ReadCoils, 17, 19, 38);
			byte[] frame = new byte[] { 17, Modbus.ReadCoils, 0, 19, 0, 37, 14, 132 };
			Assert.IsFalse(transport.ChecksumsMatch(message, frame));
		}		
	}
}
