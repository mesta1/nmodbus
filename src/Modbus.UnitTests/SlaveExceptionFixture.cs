using System;
using Modbus.Message;
using NUnit.Framework;

namespace Modbus.UnitTests
{
	[TestFixture]
	public class SlaveExceptionFixture
	{
		[Test]
		public void CreateSlaveException()
		{
			SlaveException se = new SlaveException();
			Assert.AreEqual("Exception of type 'Modbus.SlaveException' was thrown.", se.Message);
		}

		[Test]
		public void CreateSlaveExceptionWithSlaveExceptionResponse()
		{
			SlaveExceptionResponse response = new SlaveExceptionResponse(12, Modbus.ReadCoils, 200);
			SlaveException se = new SlaveException(response);
			Assert.AreEqual(String.Format("Exception of type 'Modbus.SlaveException' was thrown.\r\nFunction Code: {0}\r\nException Code: {1}", response.FunctionCode, response.SlaveExceptionCode), se.Message);
		}

		[Test]
		public void CreateSlaveExceptionWithCustomMessageAndSlaveExceptionResponse()
		{
			SlaveExceptionResponse response = new SlaveExceptionResponse(12, Modbus.ReadCoils, 200);
			string customMessage = "custom message";
			SlaveException se = new SlaveException(customMessage, response);
			Assert.AreEqual(String.Format("{0}\r\nFunction Code: {1}\r\nException Code: {2}", customMessage, response.FunctionCode, response.SlaveExceptionCode), se.Message);
		}
	}
}
