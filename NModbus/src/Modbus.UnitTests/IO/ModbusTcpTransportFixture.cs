using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Modbus.Data;
using Modbus.IO;
using Modbus.Message;
using NUnit.Framework;
using Modbus.Util;

namespace Modbus.UnitTests.IO
{
	[TestFixture]
	public class ModbusTcpTransportFixture
	{
		[Test]
		public void BuildMessageFrame()
		{
			ReadCoilsInputsRequest message = new ReadCoilsInputsRequest(Modbus.ReadCoils, 2, 10, 5);
			Assert.AreEqual(new byte[] { 0, 0, 0, 0, 0, 6, 2, 1, 0, 10, 0, 5 }, new ModbusTcpTransport().BuildMessageFrame(message));
		}

		[Test]
		public void GetMbapHeader()
		{
			WriteMultipleRegistersRequest message = new WriteMultipleRegistersRequest(3, 1, CollectionUtil.CreateDefaultCollection<RegisterCollection, ushort>(0, 120));
			byte[] header = ModbusTcpTransport.GetMbapHeader(message);
			Assert.AreEqual(new byte[] { 0, 0, 0, 0, 0, 247, 3}, header);
		}		
	}
}
