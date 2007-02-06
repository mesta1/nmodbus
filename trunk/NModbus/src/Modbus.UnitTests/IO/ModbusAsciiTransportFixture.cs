using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.IO;
using Modbus.Message;
using Rhino.Mocks;
using System.IO.Ports;
using System.IO;
using Modbus.Data;
using Modbus.UnitTests.Message;
using Modbus.Util;

namespace Modbus.UnitTests.IO
{
	[TestFixture]
	public class ModbusAsciiTransportFixture : ModbusMessageFixture
	{
		[Test]
		public void BuildMessageFrame()
		{
			byte[] message = { 58, 48, 50, 48, 49, 48, 48, 48, 48, 48, 48, 48, 49, 70, 67, 13, 10 };
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

		// TODO refactor, this pattern does not work for rtu b/c there is not a stringreader.read(byte[] method
		[Test, ExpectedException(typeof(IOException))]
		public void ReadNotEnoughBytes()
		{
			MockRepository mocks = new MockRepository();
			TextReader reader = mocks.CreateMock<StringReader>("");
			ModbusAsciiTransport transport = new ModbusAsciiTransport();
			transport.Reader = reader;
			Expect.Call(reader.ReadLine()).Return(":10");
			mocks.ReplayAll();
			transport.ReadResponse();
			mocks.VerifyAll();
		}

		[Test]
		public void Read()
		{
			MockRepository mocks = new MockRepository();
			TextReader reader = mocks.CreateMock<StringReader>("");
			ModbusAsciiTransport transport = new ModbusAsciiTransport();
			transport.Reader = reader;
			Expect.Call(reader.ReadLine()).Return(":110100130025B6");
			mocks.ReplayAll();
			transport.ReadResponse();
			mocks.VerifyAll();
		}

		[Test]
		public void ChecksumsMatchSucceed()
		{
			ModbusAsciiTransport transport = new ModbusAsciiTransport();
			ReadCoilsInputsRequest message = new ReadCoilsInputsRequest(Modbus.ReadCoils, 17, 19, 37);
			byte[] frame = { 17, Modbus.ReadCoils, 0, 19, 0, 37, 182 };
			Assert.IsTrue(transport.ChecksumsMatch(message, frame));
		}

		[Test]
		public void ChecksumsMatchFail()
		{
			ModbusAsciiTransport transport = new ModbusAsciiTransport();
			ReadCoilsInputsRequest message = new ReadCoilsInputsRequest(Modbus.ReadCoils, 17, 19, 37);
			byte[] frame = { 17, Modbus.ReadCoils, 0, 19, 0, 37, 181 };
			Assert.IsFalse(transport.ChecksumsMatch(message, frame));
		}
	}
}
