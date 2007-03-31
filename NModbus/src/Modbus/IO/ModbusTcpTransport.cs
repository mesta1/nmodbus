using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Message;
using System.Net.Sockets;
using Modbus.Util;
using System.Net;
using log4net;

namespace Modbus.IO
{
	class ModbusTcpTransport : ModbusTransport
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(ModbusTcpTransport));		
		private TcpTransportAdapter _tcpTransportAdapter;

		public ModbusTcpTransport()
		{
		}

		public ModbusTcpTransport(TcpClient tcpClient)
		{
			_tcpTransportAdapter = new TcpTransportAdapter(tcpClient.GetStream());	
		}

		public NetworkStream NetworkStream
		{
			get { return _tcpTransportAdapter.NetworkStream; }
			set { _tcpTransportAdapter.NetworkStream = value; }
		}

		public static byte[] GetMbapHeader(IModbusMessage message)
		{
			byte[] mbapHeader = { 0, 0, 0, 0, 0, 0, 0 };
			byte[] length = BitConverter.GetBytes((short) IPAddress.HostToNetworkOrder((short) (message.ProtocolDataUnit.Length + 1)));
			mbapHeader[4] = length[0];
			mbapHeader[5] = length[1];
			mbapHeader[6] = message.SlaveAddress;

			return mbapHeader;
		}

		internal override void Write(IModbusMessage message)
		{			
			byte[] frame = BuildMessageFrame(message);
			_tcpTransportAdapter.Write(frame, 0, frame.Length);
		}

		internal override byte[] BuildMessageFrame(IModbusMessage message)
		{
			List<byte> messageBody = new List<byte>();
			messageBody.AddRange(GetMbapHeader(message));
			messageBody.AddRange(message.ProtocolDataUnit);
			
			byte[] frame = messageBody.ToArray();
			return frame;
		}

		internal override byte[] ReadResponse()
		{
			return ReadRequestResponse(_tcpTransportAdapter);
		}

		internal override byte[] ReadRequest()
		{
			return ReadRequestResponse(_tcpTransportAdapter);
		}

		public static byte[] ReadRequestResponse(TcpTransportAdapter tcpTransportAdapter)
		{
			// read header
			byte[] mbapHeader = new byte[6];
			int numBytesRead = 0;
			while (numBytesRead != 6)
				numBytesRead += tcpTransportAdapter.Read(mbapHeader, numBytesRead, 6 - numBytesRead);

			_log.DebugFormat("MBAP header: {0}", StringUtil.Join(", ", mbapHeader));
			
			ushort frameLength = (ushort) (IPAddress.HostToNetworkOrder(BitConverter.ToInt16(mbapHeader, 4)));
			_log.DebugFormat("{0} bytes in PDU.", frameLength);

			// read message
			byte[] frame = new byte[frameLength];
			numBytesRead = 0;
			while (numBytesRead != frameLength)
				numBytesRead += tcpTransportAdapter.Read(frame, numBytesRead, frameLength - numBytesRead);

			_log.DebugFormat("PDU: {0}", StringUtil.Join(", ", frame));

			return frame;
		}
	}
}
