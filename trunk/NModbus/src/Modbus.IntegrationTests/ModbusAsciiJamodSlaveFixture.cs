using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Device;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace Modbus.IntegrationTests
{
	[TestFixture]
	public class ModbusAsciiJamodSlaveFixture : ModbusMasterFixture
	{		
		private string program = String.Format("SerialSlave {0} ASCII", DefaultSlaveSerialPortName);

		[TestFixtureSetUp]
		public override void Init()
		{
			base.Init();
			
			StartJamodSlave(program);

			SetupMasterSerialPort(ModbusMasterFixture.DefaultMasterSerialPortName);
			Master = ModbusSerialMaster.CreateAscii(MasterSerialPort);
		}

		[Test]
		public override void ReadCoils()
		{
			base.ReadCoils();
		}
	}
}
