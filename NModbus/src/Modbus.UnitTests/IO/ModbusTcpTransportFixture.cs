using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Modbus.Data;
using Modbus.IO;
using Modbus.Message;
using NUnit.Framework;
using Modbus.Util;
using Rhino.Mocks;

namespace Modbus.UnitTests.IO
{
	[TestFixture]
	public class ModbusTcpTransportFixture
	{
		[Test]
		public void BuildMessageFrame()
		{
			ReadCoilsInputsRequest message = new ReadCoilsInputsRequest(Modbus.ReadCoils, 2, 10, 5);
			Assert.AreEqual(new byte[] { 0, 0, 0, 0, 0, 6, 2, 1, 0, 10, 0, 5 }, new ModbusTcpTransport().BuildMessageFrame(message));
		}

		[Test]
		public void GetMbapHeader()
		{
			WriteMultipleRegistersRequest message = new WriteMultipleRegistersRequest(3, 1, CollectionUtil.CreateDefaultCollection<RegisterCollection, ushort>(0, 120));
			byte[] header = ModbusTcpTransport.GetMbapHeader(message);
			Assert.AreEqual(new byte[] { 0, 0, 0, 0, 0, 247, 3 }, header);
		}

		[Test]
		public void Write()
		{
			MockRepository mocks = new MockRepository();
			TcpStreamAdapter mockTransport = mocks.CreateMock<TcpStreamAdapter>();
			ReadCoilsInputsRequest request = new ReadCoilsInputsRequest(Modbus.ReadCoils, 1, 1, 3);
			mockTransport.Write(new byte[] { 0, 0, 0, 0, 0, 6, 1, 1, 0, 1, 0, 3 }, 0, 12);
			mocks.ReplayAll();
			new ModbusTcpTransport(mockTransport).Write(request);
			mocks.VerifyAll();
		}

		[Test]
		public void ReadRequestResponse()
		{
			MockRepository mocks = new MockRepository();
			TcpStreamAdapter mockTransport = mocks.CreateMock<TcpStreamAdapter>(null);

			byte[] mbapHeader = { 4, 5, 0, 0, 0, 6 };

			Expect.Call(mockTransport.Read(new byte[6], 0, 6)).Do(((StreamReadWriteDelegate) delegate(byte[] buf, int offset, int count)
			{
				Array.Copy(mbapHeader, buf, 6);
				return 6;
			}));

			ReadCoilsInputsRequest request = new ReadCoilsInputsRequest(Modbus.ReadCoils, 1, 1, 3);

			Expect.Call(mockTransport.Read(new byte[6], 0, 6)).Do(((StreamReadWriteDelegate) delegate(byte[] buf, int offset, int count)
			{
				Array.Copy(CollectionUtil.Combine(new byte[] { 1 }, request.ProtocolDataUnit), buf, 6);
				return 6;
			}));

			mocks.ReplayAll();
			Assert.AreEqual(ModbusTcpTransport.ReadRequestResponse(mockTransport), new byte[] { 1, 1, 0, 1, 0, 3 });
			mocks.VerifyAll();
		}
	}
}
