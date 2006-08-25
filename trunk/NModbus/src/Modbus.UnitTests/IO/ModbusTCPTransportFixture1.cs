using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.IO;
using Modbus.Message;

namespace Modbus.UnitTests.IO
{
	[TestFixture]
	public class ModbusTCPTransportFixture1
	{
		[Test]
		public void BuildMessageFrame()
		{
			ReadCoilsInputsRequest message = new ReadCoilsInputsRequest(Modbus.ReadCoils, 2, 10, 5);
			Assert.AreEqual(new byte[] { 0, 0, 0, 0, 0, 6, Byte.MaxValue, 1, 0, 10, 0, 5 }, new ModbusTCPTransport1().BuildMessageFrame(message));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ModbusTCPTranpsortNullSocket()
		{
			ModbusTCPTransport1 transport = new ModbusTCPTransport1(null);
			Assert.Fail();
		}
	}
}
