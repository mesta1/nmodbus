using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Data;
using Modbus.Message;
using NUnit.Framework;

namespace Modbus.UnitTests.Message
{
	[TestFixture]
	public class ReadInputsResponseFixture
	{
		[Test]
		public void CreateReadInputsResponse()
		{
			ReadInputsResponse response = new ReadInputsResponse(5, 2, new DiscreteCollection(true, true, true, true, true, true, false, false, true, true, false));
			Assert.AreEqual(Modbus.ReadInputs, response.FunctionCode);
			Assert.AreEqual(5, response.SlaveAddress);
			Assert.AreEqual(2, response.ByteCount);
			DiscreteCollection col = new DiscreteCollection(true, true, true, true, true, true, false, false, true, true, false);
			Assert.AreEqual(col.NetworkBytes, response.Data.NetworkBytes);
		}
	}
}
