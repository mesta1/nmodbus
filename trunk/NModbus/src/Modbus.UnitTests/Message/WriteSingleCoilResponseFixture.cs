using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Message;

namespace Modbus.UnitTests.Message
{
	public class WriteSingleCoilResponseFixture
	{
		[Test]
		public void CheckCreateWriteSingleCoilResponse()
		{
			WriteSingleCoilResponse response = new WriteSingleCoilResponse(11, 45, true);
			Assert.AreEqual(11, response.SlaveAddress);
			Assert.AreEqual(45, response.StartAddress);
			Assert.AreEqual(Modbus.CoilOn, response.Data[0]);
		}
	}
}
