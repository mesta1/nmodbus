using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using Modbus.IO;
using Modbus.Message;
using Modbus.Util;

namespace Modbus.Device
{
	public class ModbusSerialSlave : ModbusSlave
	{
		ModbusTransport _transport;

		private ModbusSerialSlave(byte unitID, ModbusTransport transport)
			: base(unitID)
		{
			_transport = transport;
		}

		public static ModbusSerialSlave CreateAscii(byte unitID, SerialPort serialPort)
		{
			return new ModbusSerialSlave(unitID, new ModbusAsciiTransport(serialPort));
		}

		public static ModbusSerialSlave CreateRtu(byte unitID, SerialPort serialPort)
		{
			return new ModbusSerialSlave(unitID, new ModbusRtuTransport(serialPort));
		}

		public ModbusTransport Transport
		{
			get { return _transport; }
			set { _transport = value; }
		}

		public override void Listen()
		{
			while (true)
			{
				try
				{
					// use transport to retrieve raw message frame from stream
					byte[] frame = Transport.ReadRequest();

					// build request from frame
					IModbusMessage request = ModbusMessageFactory.CreateModbusRequest(frame);
					log.DebugFormat("RX: {0}", StringUtil.Join(", ", request.MessageFrame));

					// only service requests addressed to this particular slave
					if (request.SlaveAddress != UnitID)
						continue;

					// perform action
					IModbusMessage response = ApplyRequest(request);

					// write response
					log.DebugFormat("TX: {0}", StringUtil.Join(", ", response.MessageFrame));
					Transport.Write(response);

				}
				catch (Exception e)
				{
					// TODO explicitly catch timeout exception
					log.ErrorFormat(ModbusResources.ModbusSlaveListenerException, e.Message);
				}
			}
		}
	}
}
