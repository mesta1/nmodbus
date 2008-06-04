using System;
using FtdAdapter;
using Modbus.Device;
using MbUnit.Framework;

namespace Modbus.IntegrationTests
{
	[TestFixture]
	public class NModbusUsbAsciiMasterFixture
	{
		[Test, ExpectedException(typeof(TimeoutException))]
		public void NModbusUsbAsciiMaster_ReadTimeout()
		{
			using (FtdUsbPort port = ModbusMasterFixture.CreateAndOpenUsbPort(ModbusMasterFixture.DefaultMasterUsbPortID))
			{
				IModbusSerialMaster master = ModbusSerialMaster.CreateAscii(port);
				master.ReadCoils(100, 1, 1);
			}
		}
	}
}
