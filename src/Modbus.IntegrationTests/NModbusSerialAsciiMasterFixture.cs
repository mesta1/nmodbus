using System;
using System.IO.Ports;
using MbUnit.Framework;
using Modbus.Device;

namespace Modbus.IntegrationTests
{
	[TestFixture]
	public class NModbusSerialAsciiMasterFixture
	{
		[Test, ExpectedException(typeof(TimeoutException))]
		public void NModbusAsciiMaster_ReadTimeout()
		{
			using (SerialPort port = ModbusMasterFixture.CreateAndOpenSerialPort(ModbusMasterFixture.DefaultMasterSerialPortName))
			{
				IModbusSerialMaster master = ModbusSerialMaster.CreateAscii(port);
				master.ReadCoils(100, 1, 1);
			}
		}
	}
}
