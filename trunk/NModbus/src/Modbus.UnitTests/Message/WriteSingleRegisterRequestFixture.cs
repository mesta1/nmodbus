using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Message;

namespace Modbus.UnitTests.Message
{
	[TestFixture]
	public class WriteSingleRegisterRequestFixture
	{
		[Test]
		public void NewWriteSingleRegisterFixture()
		{
			WriteSingleRegisterRequest request = new WriteSingleRegisterRequest(12, 5, 1200);
			Assert.AreEqual(12, request.SlaveAddress);
			Assert.AreEqual(5, request.StartAddress);
			Assert.AreEqual(1, request.Data.Count);
			Assert.AreEqual(1200, request.Data[0]);
		}
	}
}
