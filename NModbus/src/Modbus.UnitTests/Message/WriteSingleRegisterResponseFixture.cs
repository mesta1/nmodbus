using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Message;

namespace Modbus.UnitTests.Message
{
	[TestFixture]
	public class WriteSingleRegisterResponseFixture
	{
		[Test]
		public void NewWriteSingleResponseFixture()
		{
			WriteSingleRegisterResponse request = new WriteSingleRegisterResponse(12, 5, ushort.MaxValue);
			Assert.AreEqual(12, request.SlaveAddress);
			Assert.AreEqual(5, request.StartAddress);
			Assert.AreEqual(1, request.Data.Count);
			Assert.AreEqual(ushort.MaxValue, request.Data[0]);
		}
	}
}
