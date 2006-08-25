using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO.Ports;
using Modbus.Device;

namespace Modbus.IntegrationTests
{
	[TestFixture]
	public class ModbusASCIIMasterFixture1 : ModbusMasterFixture
	{
		[TestFixtureSetUp]
		public override void Init()
		{
			base.Init();
			Master = ModbusSerialMaster.CreateAscii(Port);
		}
	}
}
