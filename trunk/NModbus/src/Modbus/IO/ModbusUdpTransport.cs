using System;
using System.Net;
using System.Net.Sockets;
using log4net;
using Modbus.Message;
using Modbus.Util;

namespace Modbus.IO
{
	class ModbusUdpTransport : ModbusIpTransport
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(ModbusUdpTransport));
		private UdpClient _udpClient;
		private static IPEndPoint _remoteIpEndPoint;

		public ModbusUdpTransport()
		{
		}

		public ModbusUdpTransport(UdpClient udpClient)
		{
			_udpClient = udpClient;
		}

		public static byte[] ReadRequestResponse(UdpClient udpClient)
		{
			// Blocking receive of message
			byte[] frame = udpClient.Receive(ref _remoteIpEndPoint);
			_log.InfoFormat("RX: {0}", StringUtility.Join(", ", frame));

			return frame;
		}

		internal override void Write(IModbusMessage message)
		{
			byte[] frame = BuildMessageFrame(message);
			_log.InfoFormat("TX: {0}", StringUtility.Join(", ", frame));

			// Check if the udpClient is connected
			if (_udpClient.Client.Connected)
				_udpClient.Send(frame, frame.Length);
			else
				_udpClient.Send(frame, frame.Length, _remoteIpEndPoint);
		}

		internal override byte[] ReadRequest()
		{
			return ReadRequestResponse(_udpClient);
		}

		internal override IModbusMessage ReadResponse<T>()
		{
			return CreateMessageAndInitializeTransactionID<T>(ReadRequestResponse(_udpClient));
		}
	}
}