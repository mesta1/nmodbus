using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Message;

namespace Modbus.UnitTests.Message
{
	[TestFixture]
	public class ReadCoilsInputsRequestFixture
	{
		[Test]
		public void CreateReadCoilsRequest()
		{
			ReadCoilsInputsRequest request = new ReadCoilsInputsRequest(Modbus.ReadCoils, 5, 1, 10);
			Assert.AreEqual(Modbus.ReadCoils, request.FunctionCode);
			Assert.AreEqual(5, request.SlaveAddress);
			Assert.AreEqual(1, request.StartAddress);
			Assert.AreEqual(10, request.NumberOfPoints);
		}

		[Test]
		public void CreateReadInputsRequest()
		{
			ReadCoilsInputsRequest request = new ReadCoilsInputsRequest(Modbus.ReadInputs, 5, 1, 10);
			Assert.AreEqual(Modbus.ReadInputs, request.FunctionCode);
			Assert.AreEqual(5, request.SlaveAddress);
			Assert.AreEqual(1, request.StartAddress);
			Assert.AreEqual(10, request.NumberOfPoints);
		}
	}
}
