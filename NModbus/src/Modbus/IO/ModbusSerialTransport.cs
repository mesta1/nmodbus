using System;
using System.IO;
using log4net;
using Modbus.Message;
using Modbus.Util;

namespace Modbus.IO
{
	/// <summary>
	/// Transport for Serial protocols.
	/// </summary>
	public abstract class ModbusSerialTransport : ModbusTransport
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(ModbusTransport));
		internal SerialPortAdapter _serialPortStreamAdapter;
		private bool _checkFrame = true;

		/// <summary>
		/// Initializes a new instance of the <see cref="ModbusSerialTransport"/> class.
		/// </summary>
		public ModbusSerialTransport()
		{
		}

		/// <summary>
		/// Gets or sets a value indicating whether LRC/CRC frame checking is performed on messages.
		/// </summary>
		public bool CheckFrame
		{
			get { return _checkFrame; }
			set { _checkFrame = value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ModbusSerialTransport"/> class.
		/// </summary>
		/// <param name="serialPortStreamAdapter">The serial port stream adapter.</param>
		internal ModbusSerialTransport(SerialPortAdapter serialPortStreamAdapter)
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

		internal override IModbusMessage CreateResponse<T>(byte[] frame)
		{
			IModbusMessage response = base.CreateResponse<T>(frame);

			// compare checksum
			if (CheckFrame && !ChecksumsMatch(response, frame))
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
