using System;
using System.Net;
using System.Net.Sockets;
using log4net;
using Modbus.Message;
using Modbus.Utility;

namespace Modbus.IO
{
	class ModbusTcpTransport : ModbusIpTransport
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(ModbusTcpTransport));
		private readonly TcpStreamAdapter _tcpStreamAdapter;

		/// <summary>
		/// Initializes a new instance of the <see cref="ModbusTcpTransport"/> class.
		/// </summary>
		public ModbusTcpTransport()
		{
		}

		public ModbusTcpTransport(TcpStreamAdapter tcpStreamAdapter)
		{
			_tcpStreamAdapter = tcpStreamAdapter;
		}

		public static byte[] ReadRequestResponse(TcpStreamAdapter tcpTransportAdapter)
		{
			// read header
			byte[] mbapHeader = new byte[6];
			int numBytesRead = 0;
			while (numBytesRead != 6)
			{
				numBytesRead += tcpTransportAdapter.Read(mbapHeader, numBytesRead, 6 - numBytesRead);

				if (numBytesRead == 0)
					throw new SocketException(Modbus.ConnectionAborted);
			}
			_log.DebugFormat("MBAP header: {0}", StringUtility.Join(", ", mbapHeader));

			ushort frameLength = (ushort) (IPAddress.HostToNetworkOrder(BitConverter.ToInt16(mbapHeader, 4)));
			_log.DebugFormat("{0} bytes in PDU.", frameLength);

			// read message
			byte[] messageFrame = new byte[frameLength];
			numBytesRead = 0;
			while (numBytesRead != frameLength)
			{
				numBytesRead += tcpTransportAdapter.Read(messageFrame, numBytesRead, frameLength - numBytesRead);
				
				if (numBytesRead == 0)
					throw new SocketException(Modbus.ConnectionAborted);
			}
			_log.DebugFormat("PDU: {0}", frameLength);

			byte[] frame = CollectionUtility.Concat(mbapHeader, messageFrame);
			_log.InfoFormat("RX: {0}", StringUtility.Join(", ", frame));

			return frame;
		}

		internal override void Write(IModbusMessage message)
		{			
			byte[] frame = BuildMessageFrame(message);
			_log.InfoFormat("TX: {0}", StringUtility.Join(", ", frame));			
			_tcpStreamAdapter.Write(frame, 0, frame.Length);
		}

		internal override byte[] ReadRequest()
		{
			return ReadRequestResponse(_tcpStreamAdapter);
		}		

		internal override IModbusMessage ReadResponse<T>()
		{
			return CreateMessageAndInitializeTransactionID<T>(ReadRequestResponse(_tcpStreamAdapter));
		}
	}
}
