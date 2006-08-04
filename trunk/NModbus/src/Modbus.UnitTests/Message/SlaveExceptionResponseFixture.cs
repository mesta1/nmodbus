using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Message;

namespace Modbus.UnitTests.Message
{
	[TestFixture]
	public class SlaveExceptionResponseFixture
	{
		[Test]
		public void CheckCreateSlaveExceptionResponse()
		{
			SlaveExceptionResponse response = new SlaveExceptionResponse(11, Modbus.ReadCoils, 2);
			Assert.AreEqual(11, response.SlaveAddress);
			Assert.AreEqual(Modbus.ReadCoils, response.FunctionCode);
			Assert.AreEqual(2, response.SlaveExceptionCode);
		}
	}
}
