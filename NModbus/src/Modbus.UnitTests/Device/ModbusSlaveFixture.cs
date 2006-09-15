using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Device;
using Modbus.Data;
using Modbus.Message;
using Modbus.UnitTests.Message;

namespace Modbus.UnitTests.Device
{
	[TestFixture]
	public class ModbusSlaveFixture : ModbusMessageFixture
	{
		[Test, ExpectedException(typeof(ArgumentException))]
		public void ReadCoilsInvalidType()
		{
			ModbusSlave.ReadCoils(new ReadCoilsResponse(), 0, null);			
		}

		[Test]
		public void ReadCoils()
		{
			ReadCoilsResponse expectedResponse = new ReadCoilsResponse(1, 1, new DiscreteCollection(true, false));
			ReadCoilsResponse response = ModbusSlave.ReadCoils(new ReadCoilsInputsRequest(Modbus.ReadCoils, 1, 1, 2), 1, DataStoreFactory.CreateTestDataStore());
			AssertModbusMessagePropertiesAreEqual(expectedResponse, response);
		}
	}
}
