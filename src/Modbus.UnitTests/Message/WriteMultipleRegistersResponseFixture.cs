using System;
using Modbus.Message;
using NUnit.Framework;

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

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void CreateWriteMultipleRegistersResponseTooMuchData()
		{
			new WriteMultipleRegistersResponse(1, 2, Modbus.MaximumRegisterRequestResponseSize + 1);
		}

		[Test]
		public void CreateWriteMultipleRegistersResponseMaxSize()
		{
			WriteMultipleRegistersResponse response = new WriteMultipleRegistersResponse(1, 2, Modbus.MaximumRegisterRequestResponseSize);
			Assert.AreEqual(Modbus.MaximumRegisterRequestResponseSize, response.NumberOfPoints);
		}
	}
}
