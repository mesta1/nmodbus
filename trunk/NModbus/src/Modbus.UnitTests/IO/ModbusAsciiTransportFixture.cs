using System;
using System.IO;
using System.IO.Ports;
using Modbus.IO;
using Modbus.Message;
using Modbus.UnitTests.Message;
using MbUnit.Framework;
using Rhino.Mocks;
using System.Linq;
using Unme.MbUnit.Framework.Extensions;
using System.Linq.Expressions;

namespace Modbus.UnitTests.IO
{
	[TestFixture]
	public class ModbusAsciiTransportFixture : ModbusMessageFixture
	{
		[Test]
		public void GetReadLine_ResourceWithoutReadLineMethod()
		{
			var mocks = new MockRepository();
			var stream = mocks.StrictMock<IStreamResource>();
			var transport = new ModbusAsciiTransport(stream);

			// single character
			Expect.Call(stream.Read(new byte[1], 0, 1)).Do(((Func<byte[], int, int, int>)delegate(byte[] buf, int offset, int count)
			{
				Array.Copy(new byte[] { 1 }, buf, 1);
				return 1;
			}));

			// newline - 92, 114, 92, 110
			Expect.Call(stream.Read(new byte[] { 1 }, 0, 1)).Do(((Func<byte[], int, int, int>)delegate(byte[] buf, int offset, int count)
			{
				Array.Copy(new byte[] { 92 }, buf, 1);
				return 1;
			}));

			Expect.Call(stream.Read(new byte[] { 92 }, 0, 1)).Do(((Func<byte[], int, int, int>)delegate(byte[] buf, int offset, int count)
			{
				Array.Copy(new byte[] { 114 }, buf, 1);
				return 1;
			}));

			Expect.Call(stream.Read(new byte[] { 114 }, 0, 1)).Do(((Func<byte[], int, int, int>)delegate(byte[] buf, int offset, int count)
			{
				Array.Copy(new byte[] { 92 }, buf, 1);
				return 1;
			}));

			Expect.Call(stream.Read(new byte[] { 92 }, 0, 1)).Do(((Func<byte[], int, int, int>)delegate(byte[] buf, int offset, int count)
			{
				Array.Copy(new byte[] { 110 }, buf, 1);
				return 1;
			}));

			var getter = transport.GetReadLine();

			mocks.ReplayAll();

			getter.Invoke();

			mocks.VerifyAll();
		}

		[Test]
		public void GetReadLine_ResourceWithReadLineMethod()
		{
			var transport = new ModbusAsciiTransport(new TestSerialPortAdapter());

			var getter = transport.GetReadLine();
			Assert.AreEqual("FooBar", getter.Invoke());
		}

		[Test]
		public void BuildMessageFrame()
		{
			byte[] message = { 58, 48, 50, 48, 49, 48, 48, 48, 48, 48, 48, 48, 49, 70, 67, 13, 10 };
			ReadCoilsInputsRequest request = new ReadCoilsInputsRequest(Modbus.ReadCoils, 2, 0, 1);
			Assert.AreEqual(message, new ModbusAsciiTransport(MockRepository.GenerateStub<IStreamResource>()).BuildMessageFrame(request));
		}

		[Test]
		public void ReadRequestResponse()
		{
			var mocks = new MockRepository();
			var mockTransport = mocks.PartialMock<ModbusAsciiTransport>(MockRepository.GenerateStub<IStreamResource>());
			Expect.Call(mockTransport.GetReadLine()).Return(() => ":110100130025B6");
			mocks.ReplayAll();

			Assert.AreEqual(new byte[] { 17, 1, 0, 19, 0, 37, 182 }, mockTransport.ReadRequestResponse());

			mocks.VerifyAll();
		}

		[Test, ExpectedException(typeof(IOException))]
		public void ReadRequestResponseNotEnoughBytes()
		{
			MockRepository mocks = new MockRepository();
			IStreamResource mockSerialResource = mocks.StrictMock<IStreamResource>();
			Expect.Call(mockSerialResource.ReadTimeout).Return(SerialPort.InfiniteTimeout);
			mockSerialResource.WriteTimeout = 0;
			LastCall.IgnoreArguments();
			Expect.Call(mockSerialResource.WriteTimeout).Return(SerialPort.InfiniteTimeout);
			mockSerialResource.ReadTimeout = 0;
			LastCall.IgnoreArguments();

			var mockTransport = mocks.PartialMock<ModbusAsciiTransport>(MockRepository.GenerateStub<IStreamResource>());
			Expect.Call(mockTransport.GetReadLine()).Return(() => ":10");
			mocks.ReplayAll();

			mockTransport.ReadRequestResponse();

			mocks.VerifyAll();
		}
		
		[Test]
		public void ChecksumsMatchSucceed()
		{
			ModbusAsciiTransport transport = new ModbusAsciiTransport(MockRepository.GenerateStub<IStreamResource>());
			ReadCoilsInputsRequest message = new ReadCoilsInputsRequest(Modbus.ReadCoils, 17, 19, 37);
			byte[] frame = { 17, Modbus.ReadCoils, 0, 19, 0, 37, 182 };
			Assert.IsTrue(transport.ChecksumsMatch(message, frame));
		}

		[Test]
		public void ChecksumsMatchFail()
		{
			ModbusAsciiTransport transport = new ModbusAsciiTransport(MockRepository.GenerateStub<IStreamResource>());
			ReadCoilsInputsRequest message = new ReadCoilsInputsRequest(Modbus.ReadCoils, 17, 19, 37);
			byte[] frame = { 17, Modbus.ReadCoils, 0, 19, 0, 37, 181 };
			Assert.IsFalse(transport.ChecksumsMatch(message, frame));
		}

		class TestSerialPortAdapter : IStreamResource
		{
			public int InfiniteTimeout
			{
				get { throw new NotImplementedException(); }
			}

			public int ReadTimeout
			{
				get
				{
					throw new NotImplementedException();
				}
				set
				{
					throw new NotImplementedException();
				}
			}

			public int WriteTimeout
			{
				get
				{
					throw new NotImplementedException();
				}
				set
				{
					throw new NotImplementedException();
				}
			}

			public void DiscardInBuffer()
			{
				throw new NotImplementedException();
			}

			public int Read(byte[] buffer, int offset, int count)
			{
				throw new NotImplementedException();
			}

			public void Write(byte[] buffer, int offset, int count)
			{
				throw new NotImplementedException();
			}

			public string ReadLine()
			{
				return "FooBar";
			}
		}
	}
}
