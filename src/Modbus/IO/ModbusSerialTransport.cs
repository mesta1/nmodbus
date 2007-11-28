using System;
using System.IO;
using log4net;
using Modbus.Message;
using Modbus.Utility;

namespace Modbus.IO
{
	/// <summary>
	/// Transport for Serial protocols.
	/// </summary>
	public abstract class ModbusSerialTransport : ModbusTransport
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(ModbusTransport));
		internal ISerialResource _serialResource;
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
		/// <param name="serialResource">The serial resource.</param>
		internal ModbusSerialTransport(ISerialResource serialResource)
		{
			if (serialResource == null)
				throw new ArgumentNullException("serialResource");

			_serialResource = serialResource;		
		}

		internal override void Write(IModbusMessage message)
		{
			_serialResource.DiscardInBuffer();

			byte[] frame = BuildMessageFrame(message);
			_log.InfoFormat("TX: {0}", StringUtility.Join(", ", frame));
			_serialResource.Write(frame, 0, frame.Length);
		}

		internal override IModbusMessage CreateResponse<T>(byte[] frame)
		{
			IModbusMessage response = base.CreateResponse<T>(frame);

			// compare checksum
			if (CheckFrame && !ChecksumsMatch(response, frame))
			{
				string errorMessage = String.Format("Checksums failed to match {0} != {1}", StringUtility.Join(", ", response.MessageFrame), StringUtility.Join(", ", frame));
				_log.Error(errorMessage);
				throw new IOException(errorMessage);
			}

			return response;
		}

		internal abstract bool ChecksumsMatch(IModbusMessage message, byte[] messageFrame);
	}
}
