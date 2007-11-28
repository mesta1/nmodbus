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
		private static readonly ILog _log = LogManager.GetLogger(typeof(ModbusUdpSlave));
		private readonly UdpClient _client;

		private ModbusUdpSlave(byte unitID, UdpClient client)
			: base(unitID, new ModbusUdpTransport())
		{
			_client = client;
		}

		/// <summary>
		/// Modbus UDP slave factory method.
		/// </summary>
		public static ModbusUdpSlave CreateUdp(byte unitID, UdpClient client)
		{
			return new ModbusUdpSlave(unitID, client);
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
		/// Start slave listening for requests.
		/// </summary>
		public override void Listen()
		{
			_log.Debug("Start Modbus Udp Server.");
			_client.BeginReceive(ReceiveRequestCompleted, this);
		}

		internal void ReceiveRequestCompleted(IAsyncResult ar)
		{
			IPEndPoint masterEndPoint = null;
			byte[] frame;

			try
			{
				frame = _client.EndReceive(ar, ref masterEndPoint);
			}
			catch (ObjectDisposedException)
			{
				// this hapens when slave stops
				return;
			}

			ModbusUdpSlave slave = (ModbusUdpSlave) ar.AsyncState;

			_log.DebugFormat("Read Frame completed {0} bytes", frame.Length);
			_log.InfoFormat("RX: {0}", StringUtility.Join(", ", frame));

			IModbusMessage request = ModbusMessageFactory.CreateModbusRequest(CollectionUtility.Slice(frame, 6, frame.Length - 6));
			request.TransactionID = (ushort) IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 0));

			// TODO refactor
			ModbusUdpTransport transport = new ModbusUdpTransport();
			// perform action and build response
			IModbusMessage response = slave.ApplyRequest(request);
			response.TransactionID = request.TransactionID;

			// write response
			byte[] responseFrame = transport.BuildMessageFrame(response);
			_log.InfoFormat("TX: {0}", StringUtility.Join(", ", responseFrame));
			_client.BeginSend(responseFrame, responseFrame.Length, masterEndPoint, WriteResponseCompleted, null);
		}

		internal void WriteResponseCompleted(IAsyncResult ar)
		{
			_client.EndSend(ar);

			// Accept another request
			_client.BeginReceive(ReceiveRequestCompleted, this);
		}
	}
}