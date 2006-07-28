using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Message;

namespace Modbus.UnitTests.Message
{
	[TestFixture]
	public class ReadCoilsRequestFixture
	{
		[Test]
		public void CheckCreateReadCoilsRequest()
		{
			ReadCoilsRequest request = new ReadCoilsRequest(5, 1, 10);
			Assert.AreEqual(Modbus.READ_COILS, request.FunctionCode);
			Assert.AreEqual(5, request.SlaveAddress);
			Assert.AreEqual(1, request.StartAddress);
			Assert.AreEqual(10, request.NumberOfPoints);
		}
	}
}
