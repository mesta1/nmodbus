using Modbus.Message;
using NUnit.Framework;

namespace Modbus.UnitTests.Message
{
	[TestFixture]
	public class SlaveExceptionResponseFixture
	{
		[Test]
		public void CreateSlaveExceptionResponse()
		{
			SlaveExceptionResponse response = new SlaveExceptionResponse(11, Modbus.ReadCoils, 2);
			Assert.AreEqual(11, response.SlaveAddress);
			Assert.AreEqual(Modbus.ReadCoils, response.FunctionCode);
			Assert.AreEqual(2, response.SlaveExceptionCode);
		}
	}
}
