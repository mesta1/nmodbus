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
	public class ModbusASCIISlave1 : ModbusSlave
	{
		public ModbusASCIISlave1(byte unitID, SerialPort serialPort)
			: base(unitID, new ModbusASCIITransport1(serialPort))
		{
		}
	}
}
