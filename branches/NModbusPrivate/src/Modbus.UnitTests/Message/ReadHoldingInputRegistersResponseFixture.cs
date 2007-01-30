using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Message;
using Modbus.Data;
using NUnit.Framework;

namespace Modbus.UnitTests.Message
{
	[TestFixture]
	public class ReadHoldingInputRegistersResponseFixture
	{
		[Test]
		public void ReadHoldingRegistersResponse()
		{
			ReadHoldingInputRegistersResponse response = new ReadHoldingInputRegistersResponse(Modbus.ReadHoldingRegisters, 5, 2, new RegisterCollection(1, 2));
			Assert.AreEqual(Modbus.ReadHoldingRegisters, response.FunctionCode);
			Assert.AreEqual(5, response.SlaveAddress);
			Assert.AreEqual(2, response.ByteCount);
			RegisterCollection col = new RegisterCollection(1, 2);
			Assert.AreEqual(col.NetworkBytes, response.Data.NetworkBytes);
		}

		[Test]
		public void ReadInputRegistersResponse()
		{
			ReadHoldingInputRegistersResponse response = new ReadHoldingInputRegistersResponse(Modbus.ReadInputRegisters, 5, 2, new RegisterCollection(1, 2));
			Assert.AreEqual(Modbus.ReadInputRegisters, response.FunctionCode);
			Assert.AreEqual(5, response.SlaveAddress);
			Assert.AreEqual(2, response.ByteCount);
			RegisterCollection col = new RegisterCollection(1, 2);
			Assert.AreEqual(col.NetworkBytes, response.Data.NetworkBytes);
		}
	}
}
