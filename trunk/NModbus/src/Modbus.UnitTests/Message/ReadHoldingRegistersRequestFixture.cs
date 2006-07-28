using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Message;

namespace Modbus.UnitTests.Message
{
	[TestFixture]
	public class ReadHoldingRegistersRequestFixture
	{
		[Test]
		public void CheckNewReadHoldingRegistersRequest()
		{
			ReadHoldingRegistersRequest request = new ReadHoldingRegistersRequest(5, 1, 10);
			Assert.AreEqual(Modbus.READ_HOLDING_REGISTERS, request.FunctionCode);
			Assert.AreEqual(5, request.SlaveAddress);
			Assert.AreEqual(1, request.StartAddress);
			Assert.AreEqual(10, request.NumberOfPoints);
		}
	}
}
