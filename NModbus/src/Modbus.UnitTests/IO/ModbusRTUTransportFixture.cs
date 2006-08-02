using System;
using System.Collections.Generic;
using System.Text;
using Modbus.IO;
using Modbus.Message;
using NUnit.Framework;

namespace Modbus.UnitTests.IO
{
	[TestFixture]
	public class ModbusRTUTransportFixture
	{
		[Test, Ignore("TODO")]
		public void CheckCalculateChecksum()
		{
			ReadCoilsInputsRequest request = new ReadCoilsInputsRequest(Modbus.ReadCoils, 1, 1, 10);
			Assert.AreEqual(new byte[] { 59, 86 }, new ModbusRTUTransport(null).CalculateChecksum(request));
		}
	}
}
