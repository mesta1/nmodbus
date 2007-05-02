using System;
using System.IO;
using log4net;
using Modbus.Message;
using Modbus.Util;

namespace Modbus.IO
{
	abstract class ModbusSerialTransport : ModbusTransport
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(ModbusTransport));
		protected SerialPortAdapter _serialPortStreamAdapter;

		public ModbusSerialTransport()
		{
		}

		public ModbusSerialTransport(SerialPortAdapter serialPortStreamAdapter)
		{
			if (serialPortStreamAdapter == null)
				throw new ArgumentNullException("serialPortStreamAdapter");

			_serialPortStreamAdapter = serialPortStreamAdapter;		
		}

		internal override void Write(IModbusMessage message)
		{
			byte[] frame = BuildMessageFrame(message);
			_log.InfoFormat("TX: {0}", StringUtil.Join(", ", frame));
			_serialPortStreamAdapter.Write(frame, 0, frame.Length);
		}

		internal override T UnicastMessage<T>(IModbusMessage message)
		{
			_serialPortStreamAdapter.DiscardInBuffer();

			return base.UnicastMessage<T>(message);
		}

		internal override T CreateResponse<T>(byte[] frame)
		{
			T response = base.CreateResponse<T>(frame);

			// compare checksum
			if (!ChecksumsMatch(response, frame))
			{
				string errorMessage = String.Format("Checksums failed to match {0} != {1}", StringUtil.Join(", ", response.MessageFrame), StringUtil.Join(", ", frame));
				_log.Error(errorMessage);
				throw new IOException(errorMessage);
			}

			return response;
		}

		internal abstract bool ChecksumsMatch(IModbusMessage message, byte[] messageFrame);
	}
}
