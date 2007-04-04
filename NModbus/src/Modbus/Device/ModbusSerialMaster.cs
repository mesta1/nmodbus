using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using Modbus.IO;
using Modbus.Message;
using Modbus.Data;
using Modbus.Util;

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
			return new ModbusSerialMaster(new ModbusAsciiTransport(new SerialPortStreamAdapter(serialPort)));
		}

		public static ModbusSerialMaster CreateRtu(SerialPort serialPort)
		{
			return new ModbusSerialMaster(new ModbusRtuTransport(new SerialPortStreamAdapter(serialPort)));
		}
		
		/// <summary>
		/// Diagnostic function which loops back the original data.
		/// </summary>
		/// <param name="slaveAddress">Address of device to test.</param>
		/// <param name="data">Data to return.</param>
		/// <returns>Return true if slave device echoed data.</returns>
		public bool ReturnQueryData(byte slaveAddress, ushort data)
		{
			DiagnosticsRequestResponse request = new DiagnosticsRequestResponse(Modbus.DiagnosticsReturnQueryData, slaveAddress, new RegisterCollection(data));
			DiagnosticsRequestResponse response = Transport.UnicastMessage<DiagnosticsRequestResponse>(request);

			return response.Data[0] == data;
		}
	}
}
