using System;
using System.Collections.Generic;
using Modbus.Data;
using Modbus.IO;
using Modbus.Message;
using Modbus.Util;
using NUnit.Framework;
using Rhino.Mocks;

namespace Modbus.UnitTests.IO
{
	[TestFixture]
	public class ModbusTcpTransportFixture
	{
		[Test]
		public void BuildMessageFrame()
		{
			MockRepository mocks = new MockRepository();
			ModbusTcpTransport mockModbusTcpTransport = mocks.PartialMock<ModbusTcpTransport>();
			Expect.Call(mockModbusTcpTransport.GetNewTransactionID()).Return((ushort) 8);
			ReadCoilsInputsRequest message = new ReadCoilsInputsRequest(Modbus.ReadCoils, 2, 10, 5);
			mocks.ReplayAll();
			Assert.AreEqual(new byte[] { 0, 8, 0, 0, 0, 6, 2, 1, 0, 10, 0, 5 }, mockModbusTcpTransport.BuildMessageFrame(message));
			mocks.VerifyAll();
		}

		[Test]
		public void GetMbapHeader()
		{
			WriteMultipleRegistersRequest message = new WriteMultipleRegistersRequest(3, 1, CollectionUtil.CreateDefaultCollection<RegisterCollection, ushort>(0, 120));
			message.TransactionID = 45;
			Assert.AreEqual(new byte[] { 0, 45, 0, 0, 0, 247, 3 }, ModbusTcpTransport.GetMbapHeader(message));
		}

		[Test]
		public void Write()
		{
			MockRepository mocks = new MockRepository();
			TcpStreamAdapter mockTcpStreamAdapter = mocks.CreateMock<TcpStreamAdapter>();
			mockTcpStreamAdapter.Write(new byte[] { 255, 255, 0, 0, 0, 6, 1, 1, 0, 1, 0, 3 }, 0, 12);
			ModbusTcpTransport mockModbusTcpTransport = mocks.PartialMock<ModbusTcpTransport>(mockTcpStreamAdapter);
			Expect.Call(mockModbusTcpTransport.GetNewTransactionID()).Return(UInt16.MaxValue);
			mocks.ReplayAll();
			ReadCoilsInputsRequest request = new ReadCoilsInputsRequest(Modbus.ReadCoils, 1, 1, 3);
			mockModbusTcpTransport.Write(request);
			mocks.VerifyAll();
		}

		[Test]
		public void ReadRequestResponse()
		{
			MockRepository mocks = new MockRepository();
			TcpStreamAdapter mockTransport = mocks.CreateMock<TcpStreamAdapter>(null);

			byte[] mbapHeader = { 45, 63, 0, 0, 0, 6 };

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
			Assert.AreEqual(ModbusTcpTransport.ReadRequestResponse(mockTransport), new byte[] { 45, 63, 0, 0, 0, 6, 1, 1, 0, 1, 0, 3 });
			mocks.VerifyAll();
		}

		[Test]
		public void GetNewTransactionID()
		{
			ModbusTcpTransport transport = new ModbusTcpTransport();
			Dictionary<int, string> transactionIDs = new Dictionary<int, string>();

			for (int i = 0; i < UInt16.MaxValue; i++)
				transactionIDs.Add(transport.GetNewTransactionID(), String.Empty);

			Assert.AreEqual(1, transport.GetNewTransactionID());
			Assert.AreEqual(2, transport.GetNewTransactionID());
		}
	}
}
