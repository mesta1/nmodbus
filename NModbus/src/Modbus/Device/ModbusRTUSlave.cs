using System;
using System.Collections.Generic;
using System.Text;
using Modbus.IO;
using System.IO.Ports;

namespace Modbus.Device
{
	/// <summary>
	/// Modbus serial RTU slave.
	/// </summary>
	public class ModbusRTUSlave : ModbusSlave
	{
		public ModbusRTUSlave(byte unitID, SerialPort serialPort)
			: base(unitID, new ModbusRTUTransport(serialPort))
		{
		}
	}
}
