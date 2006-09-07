using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Message;

namespace Modbus.IO
{
	abstract class ModbusTransport
	{
		private int _retries = Modbus.DefaultRetries;

		public int Retries
		{
			get { return _retries; }
			set { _retries = value; }
		}

		public void BroadcastMessage(IModbusMessage request)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public T UnicastMessage<T>(IModbusMessage request) where T : IModbusMessage, new()
		{
			T response = default(T);

			int attempt = 1;
			bool success = false;

			do
			{
				try
				{
					Write(request);
					response = Read<T>(request);
					success = true;
				}

				catch (Exception ioe)
				{
					if (attempt++ >= _retries)
						throw ioe;
				}
			} while (!success);

			return response;
		}

		public abstract byte[] BuildMessageFrame(IModbusMessage message);
		public abstract void Close();
		public abstract byte[] GetMessageFrame();
		public abstract T Read<T>(IModbusMessage request) where T : IModbusMessage, new();
		public abstract void Write(IModbusMessage message);		
	}
}
