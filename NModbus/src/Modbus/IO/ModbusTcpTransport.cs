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

		public ModbusTcpTransport()
		{
		}

		public ModbusTcpTransport(Socket socket)
		{
			if (socket == null)
				throw new ArgumentNullException("socket");
			
			_socket = socket;
			_socket.SendTimeout = Modbus.DefaultTimeout;
			_socket.ReceiveTimeout = Modbus.DefaultTimeout;
		}

		public Socket Socket
		{
			get { return _socket; }
			set { _socket = value; }
		}

		public override void Close()
		{
			_socket.Close();
		}

		public override T Read<T>(IModbusMessage request)
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

			// check for slave exception response
			if (frame[1] > Modbus.ExceptionOffset)
				throw new SlaveException(ModbusMessageFactory.CreateModbusMessage<SlaveExceptionResponse>(frame));

			// create message from frame
			T response = ModbusMessageFactory.CreateModbusMessage<T>(frame);

			return response;
		}

		public override void Write(IModbusMessage message)
		{			
			Socket.Send(BuildMessageFrame(message));
		}

		public override byte[] BuildMessageFrame(IModbusMessage message)
		{
			byte[] mbapHeader = new byte[] { 0, 0, 0, 0, 0, (byte) (message.ProtocolDataUnit.Length + 1), Byte.MaxValue };

			List<byte> messageBody = new List<byte>();
			messageBody.AddRange(mbapHeader);
			messageBody.AddRange(message.ProtocolDataUnit);
			
			byte[] frame = messageBody.ToArray();
			return frame;
		}

		public override byte[] GetMessageFrame()
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
