using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using Modbus.IO;
using Modbus.Message;

namespace Modbus.UnitTests.IO
{
	[TestFixture]
	public class ModbusTransportFixture
	{
		// TODO implement test
		//[Test]
		//public void UnicastMessage()
		//{
		//    MockRepository mocks = new MockRepository();
		//    ModbusTransport transport = mocks.PartialMock<ModbusTransport>();
		//    transport.Write(null);
		//    LastCall.IgnoreArguments();
		//    Expect.Call(transport.ReadResponse()).Return(new byte[] { });			
		//    mocks.ReplayAll();

		//    ReadCoilsInputsRequest request = new ReadCoilsInputsRequest(Modbus.ReadCoils, 1, 5, 2);
		//    transport.UnicastMessage<ReadCoilsInputsRequest>(request);
			
		//    mocks.VerifyAll();
		//}
	}
}
