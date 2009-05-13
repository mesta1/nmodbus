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
