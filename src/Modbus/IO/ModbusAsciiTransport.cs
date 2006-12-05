using System.IO;
using Modbus.Message;
using System.IO.Ports;
using System.Text;
using Modbus.Util;
using System;
using System.Collections.Generic;
using System.Threading;
using log4net;

namespace Modbus.IO
{
	class ModbusAsciiTransport : ModbusSerialTransport
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(ModbusAsciiTransport));
		private const string FrameEnd = "\r\n";

		internal ModbusAsciiTransport()
		{
		}

		internal ModbusAsciiTransport(SerialPort serialPort)
			: base(serialPort)
		{
			SerialPort.NewLine = FrameEnd;
		}

		internal override byte[] BuildMessageFrame(IModbusMessage message)
		{
			List<byte> frame = new List<byte>();
			frame.Add((byte) ':');
			frame.AddRange(ModbusUtil.GetAsciiBytes(message.SlaveAddress));
			frame.AddRange(ModbusUtil.GetAsciiBytes(message.ProtocolDataUnit));
			frame.AddRange(ModbusUtil.GetAsciiBytes(ModbusUtil.CalculateLrc(message.MessageFrame)));
			frame.AddRange(ModbusUtil.GetAsciiBytes(FrameEnd.ToCharArray()));

			return frame.ToArray();
		}

		internal override bool ChecksumsMatch(IModbusMessage message, byte[] messageFrame)
		{
			return ModbusUtil.CalculateLrc(message.MessageFrame) == messageFrame[messageFrame.Length - 1];
		}

		internal override byte[] ReadResponse()
		{
			return ReadRequestResponse();
		}

		internal override byte[] ReadRequest()
		{
			return ReadRequestResponse();
		}

		internal byte[] ReadRequestResponse()
		{
			// read message frame, removing frame start ':'
			string frameHex = Reader.ReadLine().Substring(1);

			// convert hex to bytes
			byte[] frame = ModbusUtil.HexToBytes(frameHex);
			_log.DebugFormat("Read message frame {0}", StringUtil.Join(", ", frame));

			if (frame.Length < 3)
				throw new IOException("Premature end of stream, message truncated.");

			return frame;
		}
	}		
}
