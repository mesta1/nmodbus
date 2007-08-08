using Modbus.Device;
using NUnit.Framework;

namespace Modbus.IntegrationTests
{
	[TestFixture]
	public class ModbusUsbPortRtuFixture : ModbusMasterFixture
	{
		[TestFixtureSetUp]
		public override void Init()
		{
			base.Init();

			SetupSlaveSerialPort();
			Slave = ModbusSerialSlave.CreateRtu(SlaveAddress, SlaveSerialPort);
			StartSlave();

			SetupMasterUsbPort(DefaultMasterUsbPortID);
			Master = ModbusSerialMaster.CreateRtu(MasterUsbPort);
		}

		[Test]
		public override void ReadCoils()
		{
			base.ReadCoils();
		}
	}
}
