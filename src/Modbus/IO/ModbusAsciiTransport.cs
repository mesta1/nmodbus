using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using log4net;
using Modbus.Message;
using Modbus.Utility;

namespace Modbus.IO
{
	class ModbusAsciiTransport : ModbusSerialTransport
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(ModbusAsciiTransport));

		internal ModbusAsciiTransport()
		{
		}

		internal ModbusAsciiTransport(ISerialResource serialResource)
			: base(serialResource)
		{
			serialResource.NewLine = Environment.NewLine;
		}

		internal override byte[] BuildMessageFrame(IModbusMessage message)
		{
			List<byte> frame = new List<byte>();
			frame.Add((byte) ':');
			frame.AddRange(ModbusUtility.GetAsciiBytes(message.SlaveAddress));
			frame.AddRange(ModbusUtility.GetAsciiBytes(message.ProtocolDataUnit));
			frame.AddRange(ModbusUtility.GetAsciiBytes(ModbusUtility.CalculateLrc(message.MessageFrame)));
			frame.AddRange(Encoding.ASCII.GetBytes(Environment.NewLine.ToCharArray()));

			return frame.ToArray();
		}

		internal override bool ChecksumsMatch(IModbusMessage message, byte[] messageFrame)
		{
			return ModbusUtility.CalculateLrc(message.MessageFrame) == messageFrame[messageFrame.Length - 1];
		}

		internal override byte[] ReadRequest()
		{
			return ReadRequestResponse();
		}

		internal override IModbusMessage ReadResponse<T>()
		{
			return CreateResponse<T>(ReadRequestResponse());
		}

		internal byte[] ReadRequestResponse()
		{
			// read message frame, removing frame start ':'
			string frameHex = _serialResource.ReadLine().Substring(1);

			// convert hex to bytes
			byte[] frame = ModbusUtility.HexToBytes(frameHex);
			_log.InfoFormat("RX: {0}", StringUtility.Join(", ", frame));

			if (frame.Length < 3)
				throw new IOException("Premature end of stream, message truncated.");

			return frame;
		}
	}		
}
