using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Message;
using Modbus.Data;
using NUnit.Framework;

namespace Modbus.UnitTests.Message
{
	[TestFixture]
	public class ReadCoilsResponseFixture
	{
		[Test]
		public void CreateReadCoilsResponse()
		{
			ReadCoilsInputsResponse response = new ReadCoilsInputsResponse(Modbus.ReadCoils, 5, 2, new DiscreteCollection(true, true, true, true, true, true, false, false, true, true, false));
			Assert.AreEqual(Modbus.ReadCoils, response.FunctionCode);
			Assert.AreEqual(5, response.SlaveAddress);
			Assert.AreEqual(2, response.ByteCount);
			DiscreteCollection col = new DiscreteCollection(true, true, true, true, true, true, false, false, true, true, false);
			Assert.AreEqual(col.NetworkBytes, response.Data.NetworkBytes);
		}

		[Test]
		public void CreateReadInputsResponse()
		{
			ReadCoilsInputsResponse response = new ReadCoilsInputsResponse(Modbus.ReadInputs, 5, 2, new DiscreteCollection(true, true, true, true, true, true, false, false, true, true, false));
			Assert.AreEqual(Modbus.ReadInputs, response.FunctionCode);
			Assert.AreEqual(5, response.SlaveAddress);
			Assert.AreEqual(2, response.ByteCount);
			DiscreteCollection col = new DiscreteCollection(true, true, true, true, true, true, false, false, true, true, false);
			Assert.AreEqual(col.NetworkBytes, response.Data.NetworkBytes);
		}
	}
}
