using System;
using System.IO;
using System.IO.Ports;
using log4net;
using Modbus.IO;
using Modbus.Message;

namespace Modbus.Device
{
	/// <summary>
	/// Modbus serial slave device.
	/// </summary>
	public class ModbusSerialSlave : ModbusSlave
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(ModbusSerialSlave));

		private ModbusSerialSlave(byte unitID, ModbusTransport transport)
			: base(unitID, transport)
		{
		}

		/// <summary>
		/// Modbus ASCII slave factory method.
		/// </summary>
		public static ModbusSerialSlave CreateAscii(byte unitID, SerialPort serialPort)
		{
			return new ModbusSerialSlave(unitID, new ModbusAsciiTransport(new CommPortAdapter(serialPort)));
		}

		/// <summary>
		/// Modbus RTU slave factory method.
		/// </summary>
		public static ModbusSerialSlave CreateRtu(byte unitID, SerialPort serialPort)
		{
			return new ModbusSerialSlave(unitID, new ModbusRtuTransport(new CommPortAdapter(serialPort)));
		}

		/// <summary>
		/// Start slave listening for requests.
		/// </summary>
		public override void Listen()
		{
			try
			{
				while (true)
				{
					// read request and build message
					byte[] frame = Transport.ReadRequest();
					IModbusMessage request = ModbusMessageFactory.CreateModbusRequest(frame);

					// only service requests addressed to this particular slave
					if (request.SlaveAddress != UnitID)
					{
						_log.DebugFormat("NModbus Slave {0} ignoring request intended for NModbus Slave {1}", UnitID, request.SlaveAddress);
						continue;
					}

					// perform action
					IModbusMessage response = ApplyRequest(request);

					// write response
					Transport.Write(response);
				}
			}
			catch (IOException ioe)
			{
				_log.ErrorFormat("IO Exception encountered while listening for requests - {0}", ioe.Message);
			}
			catch (TimeoutException te)
			{
				_log.ErrorFormat("Timeout Exception encountered while listening for requests - {0}", te.Message);
			}
		}
	}
}
