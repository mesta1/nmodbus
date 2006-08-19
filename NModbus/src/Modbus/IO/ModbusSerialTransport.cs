using System.IO;
using System.IO.Ports;
using System.Reflection;
using log4net;
using Modbus.Message;
using System;

namespace Modbus.IO
{
	abstract class ModbusSerialTransport : IModbusTransport
	{
		protected static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private int _retries = Modbus.DefaultRetries;
		private SerialPort _serialPort;
		
		public ModbusSerialTransport(SerialPort serialPort)
		{
			if (serialPort == null)
				throw new ArgumentNullException("serialPort");

			_serialPort = serialPort;
		}

		public int Retries
		{
			get { return _retries; }
			set { _retries = value; }
		}

		public SerialPort SerialPort
		{
			get { return _serialPort;}
			set { _serialPort = value;}
		}

		public void Close()
		{
			_serialPort.Close();
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
					_log.ErrorFormat("Exception occurred executing unicast request - attempt {0}\n{1}", attempt, ioe.Message);

					if (attempt++ >= _retries)
						throw ioe;
				}
			} while (!success);

			return response;
		}

		public void Write(IModbusMessage message)
		{
			byte[] frame = CreateMessageFrame(message);
			SerialPort.Write(frame, 0, frame.Length);
		}

		// TODO we may refactor this to ExecuteRequest and check function code value
		public void BroadcastMessage(IModbusMessage request)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public abstract byte[] CreateMessageFrame(IModbusMessage message);
		public abstract T Read<T>(IModbusMessage request) where T : IModbusMessage, new();
	}
}
