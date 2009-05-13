using System;
using System.Globalization;
using MbUnit.Framework;
using Modbus.Device;

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
		/// Not supported
		/// </summary>
		public override void ReadWriteMultipleRegisters()
		{
		}

		/// <summary>
		/// Not supported
		/// </summary>
		public override void ReadWriteMultipleRegisters_WriteOccursBeforeRead()
		{
		}

		[Test]
		public override void ReadCoils()
		{
			base.ReadCoils();
		}
	}
}
