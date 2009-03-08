using System;
using FtdAdapter;
using MbUnit.Framework;
using Modbus.Device;

namespace Modbus.IntegrationTests
{
	[TestFixture]
	public class NModbusUsbAsciiMasterFixture
	{
		[Test, ExpectedException(typeof(TimeoutException))]
		public void NModbusUsbAsciiMaster_ReadTimeout()
		{
			using (FtdUsbPort port = ModbusMasterFixture.CreateAndOpenUsbPort(ModbusMasterFixture.DefaultMasterUsbPortId))
			{
				IModbusSerialMaster master = ModbusSerialMaster.CreateAscii(port);
				master.ReadCoils(100, 1, 1);
			}
		}
	}
}
