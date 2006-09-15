using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Data;
using Modbus.Message;

namespace Modbus.UnitTests.Message
{
	[TestFixture]
	public class WriteMultipleCoilsRequestFixture
	{
		[Test]
		public void CreateWriteMultipleCoilsRequest()
		{
			DiscreteCollection col = new DiscreteCollection(true, false, true, false, true, true, true, false, false);
			WriteMultipleCoilsRequest request = new WriteMultipleCoilsRequest(34, 45, col);
			Assert.AreEqual(Modbus.WriteMultipleCoils, request.FunctionCode);
			Assert.AreEqual(34, request.SlaveAddress);
			Assert.AreEqual(45, request.StartAddress);
			Assert.AreEqual(9, request.NumberOfPoints);
			Assert.AreEqual(2, request.ByteCount);
			Assert.AreEqual(col.NetworkBytes, request.Data.NetworkBytes);
		}
	}
}
