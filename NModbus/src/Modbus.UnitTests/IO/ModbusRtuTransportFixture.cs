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
using System.IO.Ports;

namespace Modbus.UnitTests.IO
{
	[TestFixture]
	public class ModbusRtuTransportFixture
	{
		[Test]
		public void BuildMessageFrame()
		{
		    byte[] message = { 17, Modbus.ReadCoils, 0, 19, 0, 37, 14, 132 };
		    ReadCoilsInputsRequest request = new ReadCoilsInputsRequest(Modbus.ReadCoils, 17, 19, 37);
		    Assert.AreEqual(message, new ModbusRtuTransport().BuildMessageFrame(request));
		}

		[Test]
		public void ResponseBytesToReadCoils()
		{
		    byte[] frameStart = { 0x11, 0x01, 0x05, 0xCD, 0x6B, 0xB2, 0x0E, 0x1B };
		    Assert.AreEqual(6, ModbusRtuTransport.ResponseBytesToRead(frameStart));
		}

		[Test]
		public void ResponseBytesToReadCoilsNoData()
		{
		    byte[] frameStart = { 0x11, 0x01, 0x00, 0x00, 0x00 };
		    Assert.AreEqual(1, ModbusRtuTransport.ResponseBytesToRead(frameStart));
		}

		[Test]
		public void ResponseBytesToReadWriteCoilsResponse()
		{
		    byte[] frameStart = { 0x11, 0x0F, 0x00, 0x13, 0x00, 0x0A, 0, 0 };
		    Assert.AreEqual(4, ModbusRtuTransport.ResponseBytesToRead(frameStart));
		}

		[Test]
		public void ResponseBytesToReadDiagnostics()
		{
			byte[] frameStart = { 0x01, 0x08, 0x00, 0x00 };
			Assert.AreEqual(4, ModbusRtuTransport.ResponseBytesToRead(frameStart));
		}

		[Test, ExpectedException(typeof(NotImplementedException))]
		public void ResponseBytesToReadInvalidFunctionCode()
		{
			byte[] frame = { 0x11, 0xFF, 0x00, 0x01, 0x00, 0x02, 0x04 };
			ModbusRtuTransport.ResponseBytesToRead(frame);
			Assert.Fail();
		}

		[Test]
		public void RequestBytesToReadDiagnostics()
		{
			byte[] frame = { 0x01, 0x08, 0x00, 0x00, 0xA5, 0x37, 0, 0 };
			Assert.AreEqual(1, ModbusRtuTransport.RequestBytesToRead(frame));
		}

		[Test]
		public void RequestBytesToReadCoils()
		{
			byte[] frameStart = { 0x11, 0x01, 0x00, 0x13, 0x00, 0x25 };
			Assert.AreEqual(1, ModbusRtuTransport.RequestBytesToRead(frameStart));
		}

		[Test]
		public void RequestBytesToReadWriteCoilsRequest()
		{
			byte[] frameStart = { 0x11, 0x0F, 0x00, 0x13, 0x00, 0x0A, 0x02, 0xCD, 0x01 };
			Assert.AreEqual(4, ModbusRtuTransport.RequestBytesToRead(frameStart));
		}

		[Test]
		public void RequestBytesToReadWriteMultipleHoldingRegisters()
		{
			byte[] frameStart = { 0x11, 0x10, 0x00, 0x01, 0x00, 0x02, 0x04 };
			Assert.AreEqual(6, ModbusRtuTransport.RequestBytesToRead(frameStart));
		}

		[Test, ExpectedException(typeof(NotImplementedException))]
		public void RequestBytesToReadInvalidFunctionCode()
		{
			byte[] frame = { 0x11, 0xFF, 0x00, 0x01, 0x00, 0x02, 0x04 };
			ModbusRtuTransport.RequestBytesToRead(frame);
			Assert.Fail();
		}		

		[Test]
		public void ChecksumsMatchSucceed()
		{
		    ModbusRtuTransport transport = new ModbusRtuTransport();
		    ReadCoilsInputsRequest message = new ReadCoilsInputsRequest(Modbus.ReadCoils, 17, 19, 37);
		    byte[] frame = { 17, Modbus.ReadCoils, 0, 19, 0, 37, 14, 132};
		    Assert.IsTrue(transport.ChecksumsMatch(message, frame));
		}

		[Test]
		public void ChecksumsMatchFail()
		{
		    ModbusRtuTransport transport = new ModbusRtuTransport();
		    ReadCoilsInputsRequest message = new ReadCoilsInputsRequest(Modbus.ReadCoils, 17, 19, 38);
		    byte[] frame = { 17, Modbus.ReadCoils, 0, 19, 0, 37, 14, 132 };
		    Assert.IsFalse(transport.ChecksumsMatch(message, frame));
		}

		[Test]
		public void ReadResponse()
		{
			MockRepository mocks = new MockRepository();
			ModbusRtuTransport transport = mocks.PartialMock<ModbusRtuTransport>();

			Expect.Call(transport.Read(ModbusRtuTransport.ResponseFrameStartLength))
				.Return(new byte[] { 1, 1, 1, 0 });

			Expect.Call(transport.Read(2))
				.Return(new byte[] { 81, 136 });

			mocks.ReplayAll();

			Assert.AreEqual(new byte[] { 1, 1, 1, 0, 81, 136 }, transport.ReadResponse());

			mocks.VerifyAll();
		}

		[Test]
		public void ReadRequest()
		{
			MockRepository mocks = new MockRepository();
			ModbusRtuTransport transport = mocks.PartialMock<ModbusRtuTransport>();

			Expect.Call(transport.Read(ModbusRtuTransport.RequestFrameStartLength))
				.Return(new byte[] { 1, 1, 1, 0, 1, 0, 0 });

			Expect.Call(transport.Read(1))
				.Return(new byte[] { 5 });

			mocks.ReplayAll();

			Assert.AreEqual(new byte[] { 1, 1, 1, 0, 1, 0, 0, 5 }, transport.ReadRequest());

			mocks.VerifyAll();
		}

		[Test]
		public void Read()
		{
			MockRepository mocks = new MockRepository();
			SerialPortAdapter mockSerialPort = mocks.CreateMock<SerialPortAdapter>(null);
			
			Expect.Call(mockSerialPort.Read(new byte[5], 0, 5)).Do(((StreamReadWriteDelegate) delegate(byte[] buf, int offset, int count)
			{
				Array.Copy(new byte[] { 2, 2, 2 }, buf, 3);
				return 3;
			}));

			Expect.Call(mockSerialPort.Read(new byte[] { 2, 2, 2, 0, 0 }, 3, 2)).Do(((StreamReadWriteDelegate) delegate(byte[] buf, int offset, int count)
			{
				Array.Copy(new byte[] { 3, 3 }, 0, buf, 3, 2);
				return 2;
			}));

			mocks.ReplayAll();

			ModbusRtuTransport transport = new ModbusRtuTransport(mockSerialPort);
			Assert.AreEqual(new byte[] { 2, 2, 2, 3, 3 }, transport.Read(5));

			mocks.VerifyAll();
		}
	}
}
