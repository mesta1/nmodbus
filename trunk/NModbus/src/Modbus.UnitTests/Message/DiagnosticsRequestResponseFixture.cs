using Modbus.Message;
using NUnit.Framework;
using Modbus.Data;

namespace Modbus.UnitTests.Message
{
	[TestFixture]
	public class DiagnosticsRequestResponseFixture
	{
		[Test]
		public void ToString()
		{
			var message = new DiagnosticsRequestResponse(Modbus.DiagnosticsReturnQueryData, 3, new RegisterCollection(5));

			Assert.AreEqual("Diagnostics message, sub-function return query data - {5}.", message.ToString());
		}
	}
}
