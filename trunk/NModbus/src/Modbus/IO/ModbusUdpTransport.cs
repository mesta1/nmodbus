using System;
using System.Net;
using System.Net.Sockets;
using log4net;
using Modbus.Message;
using Unme.Common;

namespace Modbus.IO
{
	class ModbusUdpTransport : ModbusIpTransport
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(ModbusUdpTransport));
		private readonly UdpClient _udpClient;

		public ModbusUdpTransport()
		{
		}

		public ModbusUdpTransport(UdpClient udpClient)
		{
			_udpClient = udpClient;
		}

		/// <summary>
		/// Blocking receive of message from default remote host
		/// </summary>
		public byte[] ReadRequestResponse(UdpClient udpClient)
		{
			if (!_udpClient.Client.Connected)
				throw new InvalidOperationException("UdpClient must be bound to a default remote host. Call the Connect method.");

			IPEndPoint remoteIpEndPoint = null;
			byte[] frame = udpClient.Receive(ref remoteIpEndPoint);
			_log.InfoFormat("RX: {0}", frame.Join(", "));

			return frame;
		}

		internal override void Write(IModbusMessage message)
		{
			if (!_udpClient.Client.Connected)
				throw new InvalidOperationException("UdpClient must be bound to a default remote host. Call the Connect method.");

			byte[] frame = BuildMessageFrame(message);
			_log.InfoFormat("TX: {0}", frame.Join(", "));
			_udpClient.Send(frame, frame.Length);
		}

		internal override byte[] ReadRequest()
		{
			return ReadRequestResponse(_udpClient);
		}

		internal override IModbusMessage ReadResponse<T>()
		{
			return CreateMessageAndInitializeTransactionId<T>(ReadRequestResponse(_udpClient));
		}
	}
}
