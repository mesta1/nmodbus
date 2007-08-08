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
		private ModbusSerialMaster(ModbusSerialTransport transport)
			: base(transport)
		{
		}

		/// <summary>
		/// Modbus ASCII master factory method.
		/// </summary>
		public static ModbusSerialMaster CreateAscii(SerialPort serialPort)
		{
			if (serialPort == null)
				throw new ArgumentNullException("serialPort");
			
			CommPortAdapter serialPortAdapter = new CommPortAdapter(serialPort);
			InitializeSerialPortTimeouts(serialPortAdapter);

			return new ModbusSerialMaster(new ModbusAsciiTransport(serialPortAdapter));
		}

		/// <summary>
		/// Modbus ASCII master factory method.
		/// </summary>
		public static ModbusSerialMaster CreateAscii(FTD2XXUsbPort usbPort)
		{
			if (usbPort == null)
				throw new ArgumentNullException("usbPort");
			
			return new ModbusSerialMaster(new ModbusAsciiTransport(new UsbPortAdapter(usbPort)));
		}

		/// <summary>
		/// Modbus RTU master factory method.
		/// </summary>
		public static ModbusSerialMaster CreateRtu(SerialPort serialPort)
		{
			if (serialPort == null)
				throw new ArgumentNullException("serialPort");

			CommPortAdapter serialPortAdapter = new CommPortAdapter(serialPort);
			InitializeSerialPortTimeouts(serialPortAdapter);

			return new ModbusSerialMaster(new ModbusRtuTransport(serialPortAdapter));
		}

		/// <summary>
		/// Modbus RTU master factory method.
		/// </summary>
		public static ModbusSerialMaster CreateRtu(FTD2XXUsbPort usbPort)
		{
			if (usbPort == null)
				throw new ArgumentNullException("usbPort");
			
			return new ModbusSerialMaster(new ModbusRtuTransport(new UsbPortAdapter(usbPort)));
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

		/// <summary>
		/// Initializes serial port read write timeouts to default value if they have not been overridden already.
		/// </summary>
		internal static void InitializeSerialPortTimeouts(ISerialResource serialResource)
		{
			serialResource.WriteTimeout = serialResource.WriteTimeout == SerialPort.InfiniteTimeout ? Modbus.DefaultTimeout : serialResource.WriteTimeout;
			serialResource.ReadTimeout = serialResource.ReadTimeout == SerialPort.InfiniteTimeout ? Modbus.DefaultTimeout : serialResource.ReadTimeout;
		}

		ModbusSerialTransport IModbusSerialMaster.Transport
		{
			get 
			{
				return (ModbusSerialTransport) Transport;
			}
		}
	}
}
