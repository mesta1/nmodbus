using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.IO;
using Modbus.Message;

namespace Modbus.UnitTests.IO
{
	[TestFixture]
	public class ModbusASCIITransportFixture
	{
		[Test]
		public void CheckCalculateRequestLRC1()
		{
			ReadCoilsRequest request = new ReadCoilsRequest(1, 1, 10);
			Assert.AreEqual(243, new ModbusASCIITransport(null).CalculateChecksum(request)[0]);
		}

		[Test]
		public void CheckCalculateRequestLRC2()
		{
			//: 02 01 0000 0001 FC
			ReadCoilsRequest request = new ReadCoilsRequest(2, 0, 1);
			Assert.AreEqual(252, new ModbusASCIITransport(null).CalculateChecksum(request)[0]);
		}

		[Test]
		public void CheckBuildASCIIMessage()
		{
			byte[] message = new byte[] { 58, 48, 50, 48, 49, 48, 48, 48, 48, 48, 48, 48, 49, 70, 67, 13, 10 };
			ReadCoilsRequest request = new ReadCoilsRequest(2, 0, 1);
			Assert.AreEqual(message, new ModbusASCIITransport(null).BuildASCIIMessage(request));
		}

		[Test, Ignore("add this later")]
		public void CheckRead()
		{
			Assert.Fail();
		}
	}
}
