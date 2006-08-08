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
	public class ModbusASCIITransport : ModbusSerialTransport, IModbusTransport
	{
		private const string FrameStart = "\r\n";

		public ModbusASCIITransport(SerialPort serialPort)
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
			frame.AddRange(ModbusUtil.GetASCIIBytes(ModbusUtil.CalculateLRC(message.ChecksumBody)));
			frame.AddRange(ModbusUtil.GetASCIIBytes(FrameStart.ToCharArray()));

			return frame.ToArray();
		}

		public override T Read<T>(IModbusMessage request)
		{
			string frame = SerialPort.ReadLine().Substring(1);

			// convert hex to bytes
			byte[] frameBytes = ModbusUtil.HexToBytes(frame);

			if (frameBytes.Length < 3)
				throw new IOException("Message was truncated.");

			// remove checksum from frame
			byte crc = frameBytes[frameBytes.Length - 1];
			Array.Resize<byte>(ref frameBytes, frameBytes.Length - 1);

			// check for slave exception response
			if (frameBytes[1] > 127)
			    throw new SlaveException(ModbusMessageFactory.CreateModbusMessage<SlaveExceptionResponse>(frameBytes));

			// create message from frame
			T response = ModbusMessageFactory.CreateModbusMessage<T>(frameBytes);

			// check LRC
			if (ModbusUtil.CalculateLRC(response.ChecksumBody) != crc)
			    throw new IOException("Checksum LRC failed.");

			return response;
		}
	}		
}
