using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Message;
using System.Net.Sockets;

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

			// read message

			// TODO 
			return default(T);
		}

		public override void Write(IModbusMessage message)
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
