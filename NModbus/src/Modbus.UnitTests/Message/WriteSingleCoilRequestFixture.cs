using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Message;

namespace Modbus.UnitTests.Message
{
	[TestFixture]
	public class WriteSingleCoilRequestFixture
	{
		[Test]
		public void CheckCreateWriteSingleCoilRequest()
		{
			WriteSingleCoilRequest request = new WriteSingleCoilRequest(11, 5, 0);
			Assert.AreEqual(11, request.SlaveAddress);
			Assert.AreEqual(5, request.StartAddress);

			// TODO store data
		}
	}
}
