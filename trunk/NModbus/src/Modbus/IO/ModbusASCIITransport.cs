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
		}

		/// <summary>
		/// Longitudinal Redundancy Check
		/// </summary>
		public override byte[] CalculateChecksum(IModbusMessage messsage)
		{
			return ModbusUtil.CalculateLRC(messsage.ChecksumBody);
		}

		public override void Write(IModbusMessage message)
		{
			byte[] messageBody = BuildASCIIMessage(message);
			SerialPort.Write(messageBody, 0, messageBody.Length);
		}

		public byte[] BuildASCIIMessage(IModbusMessage message)
		{
			List<byte> messageBody = new List<byte>();
			messageBody.Add((byte)':');
			messageBody.AddRange(ModbusUtil.GetASCIIBytes(message.SlaveAddress));
			messageBody.AddRange(ModbusUtil.GetASCIIBytes(message.ProtocolDataUnit));
			messageBody.AddRange(ModbusUtil.GetASCIIBytes(CalculateChecksum(message)));
			messageBody.AddRange(ModbusUtil.GetASCIIBytes(new char[] { '\r', '\n' }));
			
			return messageBody.ToArray();
		}

		public override T Read<T>(IModbusMessage request)
		{
			SerialPort.NewLine = FrameStart;
			string frame = SerialPort.ReadLine().Substring(1);

			// convert hex to bytes
			byte[] frameBytes = ModbusUtil.HexToBytes(frame);

			if (frameBytes.Length < 3)
				throw new IOException("Message was truncated.");

			// grab received checksum and remove from frame
			byte crc = frameBytes[frameBytes.Length - 1];
			Array.Resize<byte>(ref frameBytes, frameBytes.Length - 1);

			// check for slave exception response
			if (frameBytes[1] > 127)
			{
				SlaveExceptionResponse exceptionResponse = ModbusMessageFactory.CreateModbusMessage<SlaveExceptionResponse>(frameBytes);
			    throw new SlaveException(exceptionResponse);
			}

			// create message from frame
			T message = ModbusMessageFactory.CreateModbusMessage<T>(frameBytes);

			// check LRC
			if (CalculateChecksum(message)[0] != crc)
			    throw new IOException("Checksum LRC failed.");

			return message;
		}
	}		
}
