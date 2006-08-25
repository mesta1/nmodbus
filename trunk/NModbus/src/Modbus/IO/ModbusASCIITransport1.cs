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
	class ModbusASCIITransport1 : ModbusSerialTransport
	{
		private const string FrameStart = "\r\n";

		public ModbusASCIITransport1()
		{
		}

		public ModbusASCIITransport1(SerialPort serialPort)
			: base(serialPort)
		{
			SerialPort.NewLine = FrameStart;
		}

		public override byte[] BuildMessageFrame(IModbusMessage message)
		{
			List<byte> frame = new List<byte>();
			frame.Add((byte) ':');
			frame.AddRange(ModbusUtil.GetASCIIBytes(message.SlaveAddress));
			frame.AddRange(ModbusUtil.GetASCIIBytes(message.ProtocolDataUnit));
			frame.AddRange(ModbusUtil.GetASCIIBytes(ModbusUtil.CalculateLrc(message.ChecksumBody)));
			frame.AddRange(ModbusUtil.GetASCIIBytes(FrameStart.ToCharArray()));

			return frame.ToArray();
		}

		public override T Read<T>(IModbusMessage request)
		{
			// read message frame, removing frame start ':'
			string frameHex = SerialPort.ReadLine().Substring(1);

			// convert hex to bytes
			byte[] frame = ModbusUtil.HexToBytes(frameHex);

			if (frame.Length < 3)
				throw new IOException("Premature end of stream (Message truncated).");

			// remove checksum from frame
			byte crc = frame[frame.Length - 1];
			Array.Resize<byte>(ref frame, frame.Length - 1);

			// check for slave exception response
			if (frame[1] > Modbus.ExceptionOffset)
			    throw new SlaveException(ModbusMessageFactory.CreateModbusMessage<SlaveExceptionResponse>(frame));

			// create message from frame
			T response = ModbusMessageFactory.CreateModbusMessage<T>(frame);

			// check LRC
			if (ModbusUtil.CalculateLrc(response.ChecksumBody) != crc)
			    throw new IOException("Checksum LRC failed.");

			return response;
		}
	}		
}
