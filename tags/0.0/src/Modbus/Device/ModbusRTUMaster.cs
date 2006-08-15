using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using Modbus.IO;

namespace Modbus.Device
{
	public class ModbusRTUMaster : ModbusMaster
	{
		public ModbusRTUMaster(SerialPort serialPort)
			: base(new ModbusRTUTransport(serialPort))
		{
		}
	}
}
