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
			return new ModbusSerialMaster(new ModbusAsciiTransport(serialPort));
		}

		public static ModbusSerialMaster CreateRtu(SerialPort serialPort)
		{
			return new ModbusSerialMaster(new ModbusRtuTransport(serialPort));
		}
		
		/// <summary>
		/// Diagnostic function which loops back the original data.
		/// </summary>
		/// <param name="slaveAddress">Address of device to test.</param>
		/// <param name="data">Data to return.</param>
		/// <returns>Returns original data.</returns>
		public ushort[] ReturnQueryData(byte slaveAddress, ushort[] data)
		{
			// TODO
			//ReturnQueryDataRequestResponse request = new ReturnQueryDataRequestResponse(slaveAddress, new RegisterCollection(data));
			//ReturnQueryDataRequestResponse response = Transport.UnicastMessage<ReturnQueryDataRequestResponse>(request);
			
			//return CollectionUtil.ToArray<ushort>(response.Data);

			throw new NotImplementedException();
		}
	}
}
