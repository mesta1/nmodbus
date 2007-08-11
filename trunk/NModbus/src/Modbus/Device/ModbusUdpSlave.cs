using System;
using System.Net;
using System.Net.Sockets;
using log4net;
using Modbus.IO;
using Modbus.Message;
using Modbus.Util;
using Modbus.Utility;

namespace Modbus.Device
{
	/// <summary>
	/// Modbus UDP slave device.
	/// </summary>
	public class ModbusUdpSlave : ModbusSlave
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(ModbusUdpSlave));
		private UdpClient _client;
		private IPEndPoint _endPoint;

		private ModbusUdpSlave(byte unitID, UdpClient client, IPEndPoint endPoint)
			: base(unitID, new ModbusUdpTransport())
		{
			_client = client;
			_endPoint = endPoint;
		}

		/// <summary>
		/// Modbus UDP slave factory method.
		/// </summary>
		public static ModbusUdpSlave CreateUdp(byte unitID, UdpClient client, IPEndPoint endPoint)
		{
			return new ModbusUdpSlave(unitID, client, endPoint);
		}

		/// <summary>
		/// Start slave listening for requests.
		/// </summary>
		public override void Listen()
		{
			_log.Debug("Start Modbus Udp Server.");

			try
			{
				_client.BeginReceive(ReceiveCompleted, this);
			}
			catch (ObjectDisposedException)
			{
				// this happens when the server stops
			}
		}

		internal void ReceiveCompleted(IAsyncResult ar)
		{
			ModbusUdpSlave slave = (ModbusUdpSlave) ar.AsyncState;

			try
			{
				byte[] frame = _client.EndReceive(ar, ref _endPoint);

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
				_client.BeginSend(responseFrame, responseFrame.Length, _endPoint, WriteCompleted, null);
			}
			catch (ObjectDisposedException)
			{
				// this happens when the server stops
			}

		}

		internal void WriteCompleted(IAsyncResult ar)
		{
			_log.Debug("End write.");

			try
			{
				_client.EndSend(ar);

				// Accept another client
				_client.BeginReceive(ReceiveCompleted, this);
			}
			catch (ObjectDisposedException)
			{
				// this happens when the server stops
			}
		}
	}
}