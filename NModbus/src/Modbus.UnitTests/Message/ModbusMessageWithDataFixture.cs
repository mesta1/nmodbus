using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Message;
using Modbus.Data;

namespace Modbus.UnitTests.Message
{
	[TestFixture]
	public class ModbusMessageWithDataFixture
	{
		[Test]
		public void ModbusMessageWithDataFixtureCtorInitializesProperties()
		{
			ModbusMessageWithData<CoilDiscreteCollection> message = new ReadCoilsResponse(10, 1, new CoilDiscreteCollection(true, false, true));
			Assert.AreEqual(Modbus.ReadCoils, message.FunctionCode);
			Assert.AreEqual(10, message.SlaveAddress);
		}

		[Test]
		public void ProtocolDataUnitReadCoilsResponse()
		{
			ModbusMessageWithData<CoilDiscreteCollection> message = new ReadCoilsResponse(1, 2, new CoilDiscreteCollection(true));
			byte[] expectedResult = new byte[] { 1, 2, 1 };
			Assert.AreEqual(expectedResult, message.ProtocolDataUnit);
		}

		[Test]
		public void DataReadCoilsResponse()
		{
			CoilDiscreteCollection col = new CoilDiscreteCollection(false, true, false, true, false, true, false, false, false, false);
			ModbusMessageWithData<CoilDiscreteCollection> message = new ReadCoilsResponse(11, 1, col);
			Assert.AreEqual(col.Count, message.Data.Count);
			Assert.AreEqual(col.NetworkBytes, message.Data.NetworkBytes);
		}
	}
}
