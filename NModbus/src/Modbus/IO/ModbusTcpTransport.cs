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
		public const int MbapHeaderLength = 7;
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

		internal override void Write(IModbusMessage message)
		{			
			byte[] frame = BuildMessageFrame(message);
			NetworkStream.Write(frame, 0, frame.Length);
		}

		internal override byte[] BuildMessageFrame(IModbusMessage message)
		{
			byte[] mbapHeader = new byte[] { 0, 0, 0, 0, 0, (byte) (message.ProtocolDataUnit.Length + 1), Byte.MaxValue };

			List<byte> messageBody = new List<byte>();
			messageBody.AddRange(mbapHeader);
			messageBody.Add(message.SlaveAddress);
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
			byte[] mbapHeader = new byte[MbapHeaderLength];
			int numBytesRead = 0;
			while (numBytesRead != MbapHeaderLength)
				numBytesRead += NetworkStream.Read(mbapHeader, numBytesRead, MbapHeaderLength - numBytesRead);

			ushort frameLength = (ushort) IPAddress.HostToNetworkOrder(BitConverter.ToInt16(mbapHeader, 4));

			// read message
			byte[] frame = new byte[frameLength];
			numBytesRead = 0;
			while (numBytesRead != frameLength)
				numBytesRead += NetworkStream.Read(frame, numBytesRead, frameLength - numBytesRead);

			return frame;
		}
	}
}
