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
		private NetworkStream _networkStream;

		public ModbusTcpTransport()
		{
		}

		public ModbusTcpTransport(TcpClient tcpClient)
		{
			_networkStream = tcpClient.GetStream();
		}

		public NetworkStream NetworkStream
		{
			get { return _networkStream; }
			set { _networkStream = value; }
		}

		public static byte[] GetMbapHeader(IModbusMessage message)
		{
			byte[] mbapHeader = new byte[] { 0, 0, 0, 0, 0, 0, 0 };
			byte[] length = BitConverter.GetBytes((short) IPAddress.HostToNetworkOrder((short) (message.ProtocolDataUnit.Length + 1)));
			mbapHeader[4] = length[0];
			mbapHeader[5] = length[1];
			mbapHeader[6] = message.SlaveAddress;

			return mbapHeader;
		}

		internal override void Write(IModbusMessage message)
		{			
			byte[] frame = BuildMessageFrame(message);
			NetworkStream.Write(frame, 0, frame.Length);
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
			return ReadRequestResponse(NetworkStream);
		}

		internal override byte[] ReadRequest()
		{
			return ReadRequestResponse(NetworkStream);
		}

		public static byte[] ReadRequestResponse(NetworkStream stream)
		{
			// read header
			byte[] mbapHeader = new byte[6];
			int numBytesRead = 0;
			while (numBytesRead != 6)
				numBytesRead += stream.Read(mbapHeader, numBytesRead, 6 - numBytesRead);

			ushort frameLength = (ushort) (IPAddress.HostToNetworkOrder(BitConverter.ToInt16(mbapHeader, 4)));

			// read message
			byte[] frame = new byte[frameLength];
			numBytesRead = 0;
			while (numBytesRead != frameLength)
				numBytesRead += stream.Read(frame, numBytesRead, frameLength - numBytesRead);

			return frame;
		}
	}
}
