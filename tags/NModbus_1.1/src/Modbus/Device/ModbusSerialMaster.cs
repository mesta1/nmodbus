using System.IO.Ports;
using Modbus.Data;
using Modbus.IO;
using Modbus.Message;

namespace Modbus.Device
{
	/// <summary>
	/// Modbus serial master device.
	/// </summary>
	public class ModbusSerialMaster : ModbusMaster, IModbusMaster
	{
		private ModbusSerialMaster(ModbusTransport transport)
			: base(transport)
		{
		}

		/// <summary>
		/// Modbus ASCII master factory method.
		/// </summary>
		public static ModbusSerialMaster CreateAscii(SerialPort serialPort)
		{
			SerialPortAdapter serialPortAdapter = new SerialPortAdapter(serialPort);
			InitializeSerialPortTimeouts(serialPortAdapter);

			return new ModbusSerialMaster(new ModbusAsciiTransport(serialPortAdapter));
		}

		/// <summary>
		/// Modbus RTU master factory method.
		/// </summary>
		public static ModbusSerialMaster CreateRtu(SerialPort serialPort)
		{
			SerialPortAdapter serialPortAdapter = new SerialPortAdapter(serialPort);
			InitializeSerialPortTimeouts(serialPortAdapter);

			return new ModbusSerialMaster(new ModbusRtuTransport(new SerialPortAdapter(serialPort)));
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
		internal static void InitializeSerialPortTimeouts(SerialPortAdapter serialPortAdapter)
		{
			serialPortAdapter.WriteTimeout = serialPortAdapter.WriteTimeout == SerialPort.InfiniteTimeout ? Modbus.DefaultTimeout : serialPortAdapter.WriteTimeout;
			serialPortAdapter.ReadTimeout = serialPortAdapter.ReadTimeout == SerialPort.InfiniteTimeout ? Modbus.DefaultTimeout : serialPortAdapter.ReadTimeout;
		}
	}
}
