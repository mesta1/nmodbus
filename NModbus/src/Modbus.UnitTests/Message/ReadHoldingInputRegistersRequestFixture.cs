using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Message;

namespace Modbus.UnitTests.Message
{
	[TestFixture]
	public class ReadHoldingInputRegistersRequestFixture
	{
		[Test]
		public void CreateReadHoldingRegistersRequest()
		{
			ReadHoldingInputRegistersRequest request = new ReadHoldingInputRegistersRequest(Modbus.ReadHoldingRegisters, 5, 1, 10);
			Assert.AreEqual(Modbus.ReadHoldingRegisters, request.FunctionCode);
			Assert.AreEqual(5, request.SlaveAddress);
			Assert.AreEqual(1, request.StartAddress);
			Assert.AreEqual(10, request.NumberOfPoints);
		}

		[Test]
		public void CreateReadInputRegistersRequest()
		{
			ReadHoldingInputRegistersRequest request = new ReadHoldingInputRegistersRequest(Modbus.ReadInputRegisters, 5, 1, 10);
			Assert.AreEqual(Modbus.ReadInputRegisters, request.FunctionCode);
			Assert.AreEqual(5, request.SlaveAddress);
			Assert.AreEqual(1, request.StartAddress);
			Assert.AreEqual(10, request.NumberOfPoints);
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void CreateReadHoldingInputRegistersRequestTooMuchData()
		{
			new ReadHoldingInputRegistersRequest(Modbus.ReadHoldingRegisters, 1, 2, Modbus.MaximumRegisterRequestResponseSize + 1);
		}

		[Test]
		public void CreateReadHoldingInputRegistersRequestMaxSize()
		{
			ReadHoldingInputRegistersRequest response = new ReadHoldingInputRegistersRequest(Modbus.ReadHoldingRegisters, 1, 2, Modbus.MaximumRegisterRequestResponseSize);
			Assert.AreEqual(Modbus.MaximumRegisterRequestResponseSize, response.NumberOfPoints);
		}
	}
}
