using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Device;
using System.IO.Ports;

namespace Modbus.IntegrationTests
{
	[TestFixture]
	public class ModbusRTUMasterFixture : ModbusMasterFixture
	{
		[TestFixtureSetUp]
		public void Init()
		{
			Port.Parity = Parity.Odd;
			Master = new ModbusRTUMaster(Port);
		}
	}
}
