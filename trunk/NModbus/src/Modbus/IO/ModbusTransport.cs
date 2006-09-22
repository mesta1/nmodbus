using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using Modbus.Message;
using Modbus.Util;

namespace Modbus.IO
{
	public abstract class ModbusTransport
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(ModbusTransport));
		private int _retries = Modbus.DefaultRetries;

		public int Retries
		{
			get { return _retries; }
			set { _retries = value; }
		}

		internal void BroadcastMessage(IModbusMessage request)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		internal T UnicastMessage<T>(IModbusMessage message) where T : IModbusMessage, new()
		{
			T response = default(T);
			int attempt = 1;
			bool success = false;

			do
			{
				try
				{
					log.DebugFormat("TX: {0}", StringUtil.Join(", ", message.MessageFrame));
					Write(message);
					response = CreateResponse<T>(Read());
					log.DebugFormat("RX: {0}", StringUtil.Join(", ", response.MessageFrame));
					success = true;
				}
				catch (Exception e)	
				{
					log.DebugFormat("Failure {0}, {1} attempts remaining. {2}", attempt, _retries - attempt, e.Message);

					if (attempt++ >= _retries)
						throw e;
				}
			} while (!success);

			return response;
		}

		internal virtual T CreateResponse<T>(byte[] frame) where T : IModbusMessage, new()
		{
			byte functionCode = frame[1];

			// check for slave exception response
			if (functionCode > Modbus.ExceptionOffset)
				throw new SlaveException(ModbusMessageFactory.CreateModbusMessage<SlaveExceptionResponse>(frame));

			// create message from frame
			T response = ModbusMessageFactory.CreateModbusMessage<T>(frame);

			return response;
		}

		internal abstract byte[] BuildMessageFrame(IModbusMessage message);
		internal abstract byte[] Read();
		internal abstract void Write(IModbusMessage message);
	}
}
