using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using Modbus.IO;

namespace Modbus.Device
{
	/// <summary>
	/// Modbus serial ASCII master.
	/// </summary>
	public class ModbusASCIIMaster : ModbusMaster
	{
		public ModbusASCIIMaster(SerialPort serialPort)
			: base(new ModbusASCIITransport(serialPort))
		{
		}	
	}
}
