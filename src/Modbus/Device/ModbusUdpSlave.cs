using System;
using System.Net;
using System.Net.Sockets;
using log4net;
using Modbus.IO;
using Modbus.Message;
using Modbus.Utility;

namespace Modbus.Device
{
	/// <summary>
	/// Modbus UDP slave device.
	/// </summary>
	public class ModbusUdpSlave : ModbusSlave
	{
		private static readonly ILog _logger = LogManager.GetLogger(typeof(ModbusUdpSlave));
		private readonly UdpClient _udpClient;

		private ModbusUdpSlave(byte unitId, UdpClient udpClient)
			: base(unitId, new ModbusIpTransport(new UdpClientAdapter(udpClient)))
		{
			_udpClient = udpClient;
		}

		/// <summary>
		/// Modbus UDP slave factory method.
		/// Creates NModbus UDP slave with default
		/// </summary>
		public static ModbusUdpSlave CreateUdp(UdpClient client)
		{
			return new ModbusUdpSlave(Modbus.DefaultIpSlaveUnitId, client);
		}

		/// <summary>
		/// Modbus UDP slave factory method.
		/// </summary>
		public static ModbusUdpSlave CreateUdp(byte unitId, UdpClient client)
		{
			return new ModbusUdpSlave(unitId, client);
		}

		/// <summary>
		/// Start slave listening for requests.
		/// </summary>
		public override void Listen()
		{
			_logger.Debug("Start Modbus Udp Server.");

			try
			{
				while (true)
				{
					IPEndPoint masterEndPoint = null;
					byte[] frame;

					frame = _udpClient.Receive(ref masterEndPoint);

					_logger.DebugFormat("Read Frame completed {0} bytes", frame.Length);
					_logger.InfoFormat("RX: {0}", StringUtility.Join(", ", frame));

					IModbusMessage request = ModbusMessageFactory.CreateModbusRequest(CollectionUtility.Slice(frame, 6, frame.Length - 6));
					request.TransactionId = (ushort) IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 0));

					// perform action and build response
					IModbusMessage response = ApplyRequest(request);
					response.TransactionId = request.TransactionId;

					// write response
					byte[] responseFrame = Transport.BuildMessageFrame(response);
					_logger.InfoFormat("TX: {0}", StringUtility.Join(", ", responseFrame));
					_udpClient.Send(responseFrame, responseFrame.Length, masterEndPoint);
				}
			}
			catch (SocketException se)
			{
				// this hapens when slave stops
				if (se.ErrorCode != Modbus.WSACancelBlockingCall)
					throw;
			}
		}
	}
}
