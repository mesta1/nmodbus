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
	public class ModbusSerialMaster : ModbusMaster, IModbusMaster
	{
		private ModbusSerialMaster(ModbusTransport transport)
			: base(transport)
		{
		}

		public static ModbusSerialMaster CreateAscii(SerialPort serialPort)
		{
			return new ModbusSerialMaster(new ModbusAsciiTransport(serialPort));
		}

		public static ModbusSerialMaster CreateRtu(SerialPort serialPort)
		{
			return new ModbusSerialMaster(new ModbusRtuTransport(serialPort));
		}
	}
}
