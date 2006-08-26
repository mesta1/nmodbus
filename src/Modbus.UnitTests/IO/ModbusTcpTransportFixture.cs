using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.IO;
using Modbus.Message;

namespace Modbus.UnitTests.IO
{
	[TestFixture]
	public class ModbusTcpTransportFixture
	{
		[Test]
		public void BuildMessageFrame()
		{
			ReadCoilsInputsRequest message = new ReadCoilsInputsRequest(Modbus.ReadCoils, 2, 10, 5);
			Assert.AreEqual(new byte[] { 0, 0, 0, 0, 0, 6, Byte.MaxValue, 1, 0, 10, 0, 5 }, new ModbusTcpTransport().BuildMessageFrame(message));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ModbusTCPTranpsortNullSocket()
		{
			ModbusTcpTransport transport = new ModbusTcpTransport(null);
			Assert.Fail();
		}
	}
}
