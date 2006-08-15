using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Message;
using Modbus.Data;

namespace Modbus.UnitTests.Message
{
	[TestFixture]
	public class ModbusMessageFixture
	{
		[Test]
		public void ProtocolDataUnitReadCoilsRequest()
		{
			ModbusMessage message = new ReadCoilsInputsRequest(Modbus.ReadCoils, 1, 100, 9);
			byte[] expectedResult = new byte[] { Modbus.ReadCoils, 0, 100, 0, 9 };
			Assert.AreEqual(expectedResult, message.ProtocolDataUnit);
		}

		[Test]
		public void ChecksumBodyReadCoilsRequest()
		{
			ModbusMessage message = new ReadCoilsInputsRequest(Modbus.ReadCoils, 1, 2, 3);
			byte[] expectedChecksumBody = new byte[] { 1, Modbus.ReadCoils, 0, 2, 0, 3 };
			Assert.AreEqual(expectedChecksumBody, message.ChecksumBody);
		}

		public void AssertModbusMessagePropertiesAreEqual(ModbusMessage obj1, ModbusMessage obj2)
		{
			Assert.AreEqual(obj1.FunctionCode, obj2.FunctionCode);
			Assert.AreEqual(obj1.SlaveAddress, obj2.SlaveAddress);
			Assert.AreEqual(obj1.ChecksumBody, obj2.ChecksumBody);
			Assert.AreEqual(obj1.ProtocolDataUnit, obj2.ProtocolDataUnit);
		}
	}
}
