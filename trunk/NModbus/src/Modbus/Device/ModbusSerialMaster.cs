using System;
using System.IO.Ports;
using Modbus.Data;
using Modbus.IO;
using Modbus.Message;

namespace Modbus.Device
{
	/// <summary>
	/// Modbus serial master device.
	/// </summary>
	public class ModbusSerialMaster : ModbusMaster, IModbusSerialMaster
	{
		private ModbusSerialMaster(ModbusTransport transport)
			: base(transport)
		{
		}

		ModbusSerialTransport IModbusSerialMaster.Transport
		{
			get
			{
				return (ModbusSerialTransport) Transport;
			}
		}

		/// <summary>
		/// Modbus ASCII master factory method.
		/// </summary>
		public static ModbusSerialMaster CreateAscii(SerialPort serialPort)
		{
			if (serialPort == null)
				throw new ArgumentNullException("serialPort");

			return CreateAscii(new SerialPortAdapter(serialPort));
		}

		/// <summary>
		/// Modbus ASCII master factory method.
		/// </summary>
		public static ModbusSerialMaster CreateAscii(IStreamResource streamResource)
		{
			if (streamResource == null)
				throw new ArgumentNullException("streamResource");

			StreamResourceUtility.InitializeDefaultTimeouts(streamResource);

			return new ModbusSerialMaster(new ModbusAsciiTransport(streamResource));
		}

		/// <summary>
		/// Modbus RTU master factory method.
		/// </summary>
		public static ModbusSerialMaster CreateRtu(SerialPort serialPort)
		{
			if (serialPort == null)
				throw new ArgumentNullException("serialPort");

			return CreateRtu(new SerialPortAdapter(serialPort));
		}

		/// <summary>
		/// Modbus RTU master factory method.
		/// </summary>
		public static ModbusSerialMaster CreateRtu(IStreamResource streamResource)
		{
			if (streamResource == null)
				throw new ArgumentNullException("streamResource");

			StreamResourceUtility.InitializeDefaultTimeouts(streamResource);

			return new ModbusSerialMaster(new ModbusRtuTransport(streamResource));
		}
		
		/// <summary>
		/// Serial Line only.
		/// Diagnostic function which loops back the original data.
		/// NModbus only supports looping back one ushort value, this is a limitation of the "Best Effort" implementation of the RTU protocol.
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
