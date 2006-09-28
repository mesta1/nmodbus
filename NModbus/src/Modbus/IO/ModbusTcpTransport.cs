using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Message;
using System.Net.Sockets;
using Modbus.Util;
using System.Net;

namespace Modbus.IO
{
	class ModbusTcpTransport : ModbusTransport
	{
		private Socket _socket;

		internal ModbusTcpTransport()
		{
		}

		internal ModbusTcpTransport(Socket socket)
		{
			if (socket == null)
				throw new ArgumentNullException("socket");
			
			_socket = socket;
		}

		public Socket Socket
		{
			get { return _socket; }
			set { _socket = value; }
		}

		internal override void Write(IModbusMessage message)
		{			
			Socket.Send(BuildMessageFrame(message));
		}

		internal override byte[] BuildMessageFrame(IModbusMessage message)
		{
			byte[] mbapHeader = new byte[] { 0, 0, 0, 0, 0, (byte) (message.ProtocolDataUnit.Length + 1), Byte.MaxValue };

			List<byte> messageBody = new List<byte>();
			messageBody.AddRange(mbapHeader);
			messageBody.AddRange(message.ProtocolDataUnit);
			
			byte[] frame = messageBody.ToArray();
			return frame;
		}

		internal override byte[] ReadResponse()
		{
			return ReadRequestResponse();
		}

		internal override byte[] ReadRequest()
		{
			return ReadRequestResponse();
		}

		public byte[] ReadRequestResponse()
		{
			// read header
			byte[] MbapHeader = new byte[6];
			int numBytesRead = 0;
			while (numBytesRead != 6)
				numBytesRead += Socket.Receive(MbapHeader, numBytesRead, 6 - numBytesRead, SocketFlags.None);

			ushort frameLength = (ushort) IPAddress.HostToNetworkOrder(BitConverter.ToInt16(MbapHeader, 4));

			// read message
			byte[] frame = new byte[frameLength];
			numBytesRead = 0;
			while (numBytesRead != frameLength)
				numBytesRead += Socket.Receive(frame, numBytesRead, frameLength - numBytesRead, SocketFlags.None);

			return frame;
		}
	}
}
