using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Message;

namespace Modbus.UnitTests.Message
{
	[TestFixture]
	public class WriteMultipleRegistersResponseFixture
	{
		[Test]
		public void CreateWriteMultipleRegistersResponse()
		{
			WriteMultipleRegistersResponse response = new WriteMultipleRegistersResponse(12, 39, 2);
			Assert.AreEqual(Modbus.WriteMultipleRegisters, response.FunctionCode);
			Assert.AreEqual(12, response.SlaveAddress);
			Assert.AreEqual(39, response.StartAddress);
			Assert.AreEqual(2, response.NumberOfPoints);
		}
	}
}
