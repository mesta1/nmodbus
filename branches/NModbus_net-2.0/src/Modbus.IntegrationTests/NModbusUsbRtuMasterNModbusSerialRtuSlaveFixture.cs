using MbUnit.Framework;
using Modbus.Device;

namespace Modbus.IntegrationTests
{
	[TestFixture]
	public class NModbusUsbRtuMasterNModbusSerialRtuSlaveFixture : ModbusSerialMasterFixture
	{
		[TestFixtureSetUp]
		public override void Init()
		{
			base.Init();

			SetupSlaveSerialPort();
			Slave = ModbusSerialSlave.CreateRtu(SlaveAddress, SlaveSerialPort);
			StartSlave();

			MasterUsbPort = CreateAndOpenUsbPort(DefaultMasterUsbPortId);
			Master = ModbusSerialMaster.CreateRtu(MasterUsbPort);
		}

		/// <summary>
		/// Not implemented by slave yet
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
