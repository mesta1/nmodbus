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
			ReadCoilsResponse response = new ReadCoilsResponse(5, 2, new CoilDiscreteCollection(true, true, true, true, true, true, false, false, true, true, false));
			Assert.AreEqual(Modbus.ReadCoils, response.FunctionCode);
			Assert.AreEqual(5, response.SlaveAddress);
			Assert.AreEqual(2, response.ByteCount);
			CoilDiscreteCollection col = new CoilDiscreteCollection(true, true, true, true, true, true, false, false, true, true, false);
			Assert.AreEqual(col.NetworkBytes, response.Data.NetworkBytes);
		}
	}
}
