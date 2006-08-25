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
	public class ModbusRTUSlave1 : ModbusSlave
	{
		public ModbusRTUSlave1(byte unitID, SerialPort serialPort)
			: base(unitID, new ModbusRTUTransport1(serialPort))
		{
		}
	}
}
