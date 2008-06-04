using System;
using System.IO;
using System.IO.Ports;
using Modbus.IO;
using Modbus.Message;
using Modbus.UnitTests.Message;
using MbUnit.Framework;
using Rhino.Mocks;

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

		[Test, ExpectedArgumentNullException]
		public void ModbusASCIITranpsortNullSerialPort()
		{
			ModbusAsciiTransport transport = new ModbusAsciiTransport(null);
			Assert.Fail();
		}

		[Test]
		public void ReadRequestResponse()
		{
			MockRepository mocks = new MockRepository();
			ISerialResource mockSerialResource = mocks.CreateMock<ISerialResource>();
			mockSerialResource.NewLine = Environment.NewLine;
			Expect.Call(mockSerialResource.ReadLine()).Return(":110100130025B6");
			mocks.ReplayAll();

			ModbusAsciiTransport transport = new ModbusAsciiTransport(mockSerialResource);
			Assert.AreEqual(new byte[] { 17, 1, 0, 19, 0, 37, 182 }, transport.ReadRequestResponse());

			mocks.VerifyAll();
		}
			
		[Test, ExpectedException(typeof(IOException))]
		public void ReadRequestResponseNotEnoughBytes()
		{
			MockRepository mocks = new MockRepository(); 
			ISerialResource mockSerialResource = mocks.CreateMock<ISerialResource>();
			mockSerialResource.NewLine = Environment.NewLine;
			Expect.Call(mockSerialResource.ReadTimeout).Return(SerialPort.InfiniteTimeout);
			mockSerialResource.WriteTimeout = 0;
			LastCall.IgnoreArguments();
			Expect.Call(mockSerialResource.WriteTimeout).Return(SerialPort.InfiniteTimeout);
			mockSerialResource.ReadTimeout = 0;
			LastCall.IgnoreArguments();
			Expect.Call(mockSerialResource.ReadLine()).Return(":10");						
			mocks.ReplayAll();

			ModbusAsciiTransport transport = new ModbusAsciiTransport(mockSerialResource);			
			transport.ReadRequestResponse();

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
