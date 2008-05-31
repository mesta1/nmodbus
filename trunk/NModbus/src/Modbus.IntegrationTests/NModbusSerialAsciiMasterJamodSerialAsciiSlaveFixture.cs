using System;
using Modbus.Device;
using NUnit.Framework;
using System.Globalization;

namespace Modbus.IntegrationTests
{
	[TestFixture]
	public class NModbusSerialAsciiMasterJamodSerialAsciiSlaveFixture : ModbusMasterFixture
	{		
		private string program = String.Format(CultureInfo.InvariantCulture, "SerialSlave {0} ASCII", DefaultSlaveSerialPortName);

		[TestFixtureSetUp]
		public override void Init()
		{
			base.Init();
			
			StartJamodSlave(program);

			MasterSerialPort = CreateAndOpenSerialPort(DefaultMasterSerialPortName);
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
