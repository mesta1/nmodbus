using System.Collections.Generic;
using System.IO;
using System.Text;
using log4net;
using Modbus.Message;
using Modbus.Util;

namespace Modbus.IO
{
	class ModbusAsciiTransport : ModbusSerialTransport
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(ModbusAsciiTransport));
		public const string FrameEnd = "\r\n";

		internal ModbusAsciiTransport()
		{
		}

		internal ModbusAsciiTransport(SerialPortAdapter serialPortAdapter)
			: base(serialPortAdapter)
		{
			serialPortAdapter.NewLine = FrameEnd;
		}

		internal override byte[] BuildMessageFrame(IModbusMessage message)
		{
			List<byte> frame = new List<byte>();
			frame.Add((byte) ':');
			frame.AddRange(ModbusUtil.GetAsciiBytes(message.SlaveAddress));
			frame.AddRange(ModbusUtil.GetAsciiBytes(message.ProtocolDataUnit));
			frame.AddRange(ModbusUtil.GetAsciiBytes(ModbusUtil.CalculateLrc(message.MessageFrame)));
			frame.AddRange(Encoding.ASCII.GetBytes(FrameEnd.ToCharArray()));

			return frame.ToArray();
		}

		internal override bool ChecksumsMatch(IModbusMessage message, byte[] messageFrame)
		{
			return ModbusUtil.CalculateLrc(message.MessageFrame) == messageFrame[messageFrame.Length - 1];
		}

		internal override byte[] ReadRequest()
		{
			return ReadRequestResponse();
		}

		internal override T ReadResponse<T>()
		{
			return CreateResponse<T>(ReadRequestResponse());
		}

		internal byte[] ReadRequestResponse()
		{
			// read message frame, removing frame start ':'
			string frameHex = _serialPortStreamAdapter.ReadLine().Substring(1);

			// convert hex to bytes
			byte[] frame = ModbusUtil.HexToBytes(frameHex);
			_log.InfoFormat("RX: {0}", StringUtil.Join(", ", frame));

			if (frame.Length < 3)
				throw new IOException("Premature end of stream, message truncated.");

			return frame;
		}
	}		
}
