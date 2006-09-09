using System.IO;
using Modbus.Message;
using System.IO.Ports;
using System.Text;
using Modbus.Util;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Modbus.IO
{
	class ModbusAsciiTransport : ModbusSerialTransport
	{
		private const string FrameEnd = "\r\n";

		public ModbusAsciiTransport()
		{
		}

		public ModbusAsciiTransport(SerialPort serialPort)
			: base(serialPort)
		{
			SerialPort.NewLine = FrameEnd;
		}

		public override byte[] BuildMessageFrame(IModbusMessage message)
		{
			List<byte> frame = new List<byte>();
			frame.Add((byte) ':');
			frame.AddRange(ModbusUtil.GetASCIIBytes(message.SlaveAddress));
			frame.AddRange(ModbusUtil.GetASCIIBytes(message.ProtocolDataUnit));
			frame.AddRange(ModbusUtil.GetASCIIBytes(ModbusUtil.CalculateLrc(message.MessageFrame)));
			frame.AddRange(ModbusUtil.GetASCIIBytes(FrameEnd.ToCharArray()));

			return frame.ToArray();
		}

		public override bool ChecksumsMatch(IModbusMessage message, byte[] messageFrame)
		{
			return ModbusUtil.CalculateLrc(message.MessageFrame) == messageFrame[messageFrame.Length - 1];
		}

		public override byte[] Read()
		{
			// read message frame, removing frame start ':'
			string frameHex = Reader.ReadLine().Substring(1);

			// convert hex to bytes
			byte[] frame = ModbusUtil.HexToBytes(frameHex);

			if (frame.Length < 3)
				throw new IOException("Premature end of stream, message truncated.");

			return frame;
		}
	}		
}
