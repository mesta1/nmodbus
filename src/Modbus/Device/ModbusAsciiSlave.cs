using System;
using System.Collections.Generic;
using System.Text;
using Modbus.IO;
using System.IO.Ports;

namespace Modbus.Device
{
	/// <summary>
	/// Modbus serial ASCII slave.
	/// </summary>
	public class ModbusAsciiSlave : ModbusSlave
	{
		public ModbusAsciiSlave(byte unitID, SerialPort serialPort)
			: base(unitID, new ModbusAsciiTransport(serialPort))
		{
		}
	}
}
