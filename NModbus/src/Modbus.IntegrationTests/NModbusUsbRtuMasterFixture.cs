using System;
using FtdAdapter;
using Modbus.Device;
using MbUnit.Framework;

namespace Modbus.IntegrationTests
{
	[TestFixture]
	public class NModbusUsbRtuMasterFixture
	{
		[Test, ExpectedException(typeof(TimeoutException))]
		public void NModbusUsbRtuMaster_ReadTimeout()
		{
			using (FtdUsbPort port = ModbusMasterFixture.CreateAndOpenUsbPort(ModbusMasterFixture.DefaultMasterUsbPortId))
			{
				IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(port);
				master.ReadCoils(100, 1, 1);
			}
		}
	}
}
