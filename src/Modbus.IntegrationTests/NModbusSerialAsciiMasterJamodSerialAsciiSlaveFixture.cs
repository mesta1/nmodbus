using System;
using Modbus.Device;
using NUnit.Framework;

namespace Modbus.IntegrationTests
{
	[TestFixture]
	public class NModbusSerialAsciiMasterJamodSerialAsciiSlaveFixture : ModbusMasterFixture
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

		/// <summary>
		/// Jamod slave does not support this function
		/// </summary>
		public override void ReadWriteMultipleRegisters()
		{
		}

		[Test]
		public override void ReadCoils()
		{
			base.ReadCoils();
		}
	}
}
