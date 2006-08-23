using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Message;
using System.Net.Sockets;
using Modbus.Util;

namespace Modbus.IO
{
	class ModbusTCPTransport : ModbusTransport
	{
		private Socket _socket;

		public ModbusTCPTransport(Socket socket)
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

		public override void Close()
		{
			_socket.Close();
		}

		public override T Read<T>(IModbusMessage request)
		{
			// read header
			byte[] MbapHeader = new byte[6];
			Socket.Receive(MbapHeader);
			ushort frameLength = BitConverter.ToUInt16(MbapHeader, 4);

			// read message
			byte[] frame = new byte[frameLength];
			Socket.Receive(frame);

			// check for slave exception response
			if (frame[1] > Modbus.ExceptionOffset)
				throw new SlaveException(ModbusMessageFactory.CreateModbusMessage<SlaveExceptionResponse>(frame));

			// create message from frame
			T response = ModbusMessageFactory.CreateModbusMessage<T>(frame);

			// TODO CRC?

			return response;
		}

		public override void Write(IModbusMessage message)
		{
			List<byte> messageBody = new List<byte>();
			messageBody.AddRange(new byte[] { 0, 0, 0, 0, 0, 0 });
			messageBody.Add(message.SlaveAddress);
			messageBody.AddRange(message.ProtocolDataUnit);

			byte[] frame = messageBody.ToArray();
			Socket.Send(frame);
		}
	}
}
