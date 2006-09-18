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
			ModbusMessageWithData<DiscreteCollection> message = new ReadCoilsInputsResponse(10, 1, new DiscreteCollection(true, false, true));
			Assert.AreEqual(Modbus.ReadCoils, message.FunctionCode);
			Assert.AreEqual(10, message.SlaveAddress);
		}

		[Test]
		public void ProtocolDataUnitReadCoilsResponse()
		{
			ModbusMessageWithData<DiscreteCollection> message = new ReadCoilsInputsResponse(1, 2, new DiscreteCollection(true));
			byte[] expectedResult = new byte[] { 1, 2, 1 };
			Assert.AreEqual(expectedResult, message.ProtocolDataUnit);
		}

		[Test]
		public void DataReadCoilsResponse()
		{
			DiscreteCollection col = new DiscreteCollection(false, true, false, true, false, true, false, false, false, false);
			ModbusMessageWithData<DiscreteCollection> message = new ReadCoilsInputsResponse(11, 1, col);
			Assert.AreEqual(col.Count, message.Data.Count);
			Assert.AreEqual(col.NetworkBytes, message.Data.NetworkBytes);
		}
	}
}
