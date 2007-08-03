using System;
using System.IO;
using log4net;
using Modbus.Message;

namespace Modbus.IO
{
	/// <summary>
	/// Modbus transport.
	/// </summary>
	public abstract class ModbusTransport
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(ModbusTransport));
		private int _retries = Modbus.DefaultRetries;

		/// <summary>
		/// Number of times to retry sending message.
		/// </summary>
		public int Retries
		{
			get { return _retries; }
			set { _retries = value; }
		}

		internal virtual T UnicastMessage<T>(IModbusMessage message) where T : class, IModbusMessage, new()
		{
			IModbusMessage response = null;
			int attempt = 1;
			bool readAgain = false;
			bool success = false;

			do
			{
				try
				{
					Write(message);

					do
					{
						response = ReadResponse<T>();

						if (response is SlaveExceptionResponse)
						{
							if (response.FunctionCode == Modbus.Acknowlege)
							{
								readAgain = true;
							}
							else
							{
								throw new SlaveException((SlaveExceptionResponse) response);
							}
						}
						
					} while (readAgain);

					ValidateResponse(message, response);
					success = true;
				}
				catch (TimeoutException te)
				{
					_log.ErrorFormat("Timeout, {0} retries remaining - {1}", _retries + 1 - attempt, te.Message);

					if (attempt++ > _retries)
						throw;
				}
				catch (IOException ioe)
				{
					_log.ErrorFormat("IO Exception, {0} retries remaining - {1}", _retries + 1 - attempt, ioe.Message);

					if (attempt++ > _retries)
						throw;
				}
				catch (SlaveException se)
				{
					_log.ErrorFormat("Slave Exception, {0} retries remaining - {1}", _retries + 1 - attempt, se.Message);

					if (attempt++ > _retries)
						throw;
				}

			} while (!success);

			return (T) response;
		}

		internal virtual IModbusMessage CreateResponse<T>(byte[] frame) where T : IModbusMessage, new()
		{
			byte functionCode = frame[1];
			IModbusMessage response = null;

			// check for slave exception response
			if (functionCode > Modbus.ExceptionOffset)
				response = ModbusMessageFactory.CreateModbusMessage<SlaveExceptionResponse>(frame);
			else
				// create message from frame
				response = ModbusMessageFactory.CreateModbusMessage<T>(frame);

			return response;
		}

		internal virtual void ValidateResponse(IModbusMessage request, IModbusMessage response)
		{
			if (request.FunctionCode != response.FunctionCode)
				throw new IOException(String.Format("Received response with unexpected Function Code. Expected {0}, received {1}.", request.FunctionCode, response.FunctionCode));
		}

		internal abstract byte[] ReadRequest();
		internal abstract IModbusMessage ReadResponse<T>() where T : IModbusMessage, new();
		internal abstract byte[] BuildMessageFrame(IModbusMessage message);
		internal abstract void Write(IModbusMessage message);
	}
}
