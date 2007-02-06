using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Message;
using Modbus.Data;
using Modbus.Util;

namespace Modbus.UnitTests.Message
{
	[TestFixture]
	public class ReadWriteMultipleRegistersRequestFixture
	{
		[Test]
		public void ReadWriteMultipleRegistersRequest()
		{
			RegisterCollection writeCollection = new RegisterCollection(255, 255, 255);
			ReadWriteMultipleRegistersRequest request = new ReadWriteMultipleRegistersRequest(5, 3, 6, 14, writeCollection);
			Assert.AreEqual(Modbus.ReadWriteMultipleRegisters, request.FunctionCode);
			Assert.AreEqual(5, request.SlaveAddress);

			// TODO 
			// test start read address
			// number of points to read
			// test start write address
			// number of points to write
		}

		[Test]
		// TODO
		public void ReadWriteMultipleRegistersRequestFromFrame()
		{
			byte[] frame = { Modbus.ReadWriteMultipleRegisters };
			
		}

		[Test]
		public void ProtocolDataUnit()
		{
			RegisterCollection writeCollection = new RegisterCollection(255, 255, 255);
			ReadWriteMultipleRegistersRequest request = new ReadWriteMultipleRegistersRequest(5, 3, 6, 14, writeCollection);
			byte[] pdu = { 0x17, 0x00, 0x03, 0x00, 0x06, 0x00, 0x0e, 0x00, 0x03, 0x06, 0x00, 0xff, 0x00, 0xff, 0x00, 0xff };
			Assert.AreEqual(pdu, request.ProtocolDataUnit);
		}

		[Test]
		public void MessageFrame()
		{
			RegisterCollection writeCollection = new RegisterCollection(255, 255, 255);
			ReadWriteMultipleRegistersRequest request = new ReadWriteMultipleRegistersRequest(5, 3, 6, 14, writeCollection);
			byte[] message = CollectionUtil.Combine(new byte[] { 5 }, request.ProtocolDataUnit);
			Assert.AreEqual(new byte[] { 0x05, 0x17, 0x00, 0x03, 0x00, 0x06, 0x00, 0x0e, 0x00, 0x03, 0x06, 0x00, 0xff, 0x00, 0xff, 0x00, 0xff }, message);
		}
	}
}
