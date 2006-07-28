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
		public void CheckCreateReadCoilsResponse()
		{
			ReadCoilsResponse response = new ReadCoilsResponse(5, 2, new CoilDiscreteCollection(true, true, true, true, true, true, false, false, true, true, false));
			Assert.AreEqual(Modbus.READ_COILS, response.FunctionCode);
			Assert.AreEqual(5, response.SlaveAddress);
			Assert.AreEqual(2, response.ByteCount);
			// TODO refactor comparision to CoilDiscreteCollectionFixture
			CoilDiscreteCollection col = new CoilDiscreteCollection(true, true, true, true, true, true, false, false, true, true, false);
			Assert.AreEqual(col.Count, response.Data.Count);
			Assert.AreEqual(col.Bytes, response.Data.Bytes);
		}
	}
}
