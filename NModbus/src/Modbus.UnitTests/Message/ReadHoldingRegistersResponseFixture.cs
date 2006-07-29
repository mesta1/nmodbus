using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Message;
using Modbus.Data;

namespace Modbus.UnitTests.Message
{
	[TestFixture]
	public class ReadHoldingRegistersResponseFixture
	{
		[Test]
		public void CheckNewReadHoldingRegistersResponseFixture()
		{
			ReadHoldingRegistersResponse response = new ReadHoldingRegistersResponse(2, 3, new HoldingRegisterCollection(1, 2, 3));
			Assert.AreEqual(Modbus.ReadHoldingRegisters, response.FunctionCode);
			Assert.AreEqual(2, response.SlaveAddress);
			Assert.AreEqual(3, response.ByteCount);
			// TODO refactor comparision to HoldingRegisterCollectionFixture
			HoldingRegisterCollection col = new HoldingRegisterCollection(1, 2, 3);
			Assert.AreEqual(col.Count, response.Data.Count);
			Assert.AreEqual(col.Bytes, response.Data.Bytes);
		}
	}
}
