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

		internal override void Write(IModbusMessage message)
		{			
			byte[] frame = BuildMessageFrame(message);
			NetworkStream.Write(frame, 0, frame.Length);
		}

		internal override byte[] BuildMessageFrame(IModbusMessage message)
		{
			byte[] mbapHeader = new byte[] { 0, 0, 0, 0, 0, (byte) (message.ProtocolDataUnit.Length), message.SlaveAddress };

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
			byte[] mbapHeader = new byte[6];
			int numBytesRead = 0;
			while (numBytesRead != 6)
				numBytesRead += NetworkStream.Read(mbapHeader, numBytesRead, 6 - numBytesRead);

			ushort frameLength = (ushort) (IPAddress.HostToNetworkOrder(BitConverter.ToInt16(mbapHeader, 4)) + 1);

			// read message
			byte[] frame = new byte[frameLength];
			numBytesRead = 0;
			while (numBytesRead != frameLength)
				numBytesRead += NetworkStream.Read(frame, numBytesRead, frameLength - numBytesRead);

			return frame;
		}
	}
}
