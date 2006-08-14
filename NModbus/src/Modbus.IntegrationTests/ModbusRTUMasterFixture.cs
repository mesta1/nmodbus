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
		public override void Init()
		{
			base.Init();
			Port.Parity = Parity.Odd;
			Master = new ModbusRTUMaster(Port);
		}

		[Test]
		public override void ReadCoils()
		{
			int test = 5;
			base.ReadCoils();
		}

		[Test]
		public override void Read0Coils()
		{
			base.Read0Coils();
		}

		[Test]
		public override void  ReadHoldingRegisters()
		{
 			 base.ReadHoldingRegisters();
		}

		[Test]
		public override void WriteMultipleRegisters()
		{
			base.WriteMultipleRegisters();
		}
	}
}
