using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Modbus.IO;
using Modbus.Message;
using NUnit.Framework;
using Rhino.Mocks;


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

		//[Test]
		//[ExpectedException(typeof(ArgumentNullException))]
		//public void ModbusTCPTranpsortNullSocket()
		//{
		//    Socket sock = null;
		//    ModbusTcpTransport transport = new ModbusTcpTransport(sock);
		//    Assert.Fail();
		//}

		// TODO test read success
	}
}
