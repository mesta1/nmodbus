using System;
using System.Collections.Generic;
using System.Text;
using Modbus.IO;
using System.IO.Ports;

namespace Modbus.Device
{
	public class ModbusASCIISlave : ModbusSlave
	{
		public ModbusASCIISlave(byte unitID, SerialPort serialPort)
			: base(unitID, new ModbusASCIITransport(serialPort))
		{
		}
	}
}
