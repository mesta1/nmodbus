using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Message;
using System.IO.Ports;
using Modbus.Util;
using System.IO;
using Modbus.IO;

namespace Modbus.IO
{
	class ModbusRtuTransport : ModbusSerialTransport
	{
		public ModbusRtuTransport ()
		{
		}

		public ModbusRtuTransport(SerialPort serialPort)
			: base (serialPort)
		{
		}

		public override byte[] BuildMessageFrame(IModbusMessage message)
		{
			List<byte> messageBody = new List<byte>();
			messageBody.Add(message.SlaveAddress);
			messageBody.AddRange(message.ProtocolDataUnit);
			messageBody.AddRange(ModbusUtil.CalculateCrc(message.MessageFrame));

			return messageBody.ToArray();
		}

		public override bool ChecksumsMatch(IModbusMessage message, byte[] messageFrame)
		{
			return BitConverter.ToUInt16(messageFrame, messageFrame.Length - 2) == BitConverter.ToUInt16(ModbusUtil.CalculateCrc(message.MessageFrame), 0);
		}

		public override byte[] Read()
		{
			// read beginning of message frame
			byte[] frameStart = new byte[4];
			int numBytesRead = 0;
			while (numBytesRead != 4)
				numBytesRead += SerialPort.Read(frameStart, numBytesRead, 4 - numBytesRead);

			byte functionCode = frameStart[1];
			byte byteCount = frameStart[2];

			// calculate number of bytes remaining in message frame
			int bytesRemaining = NumberOfBytesToRead(functionCode, byteCount);

			// read remaining bytes
			byte[] frameEnd = new byte[bytesRemaining];
			numBytesRead = 0;
			while (numBytesRead != bytesRemaining)
				numBytesRead += SerialPort.Read(frameEnd, numBytesRead, bytesRemaining - numBytesRead);

			// build complete message frame
			byte[] frame = CollectionUtil.Combine<byte>(frameStart, frameEnd);

			return frame;
		}

		internal static int NumberOfBytesToRead(byte functionCode, byte byteCount)
		{
			int numBytes;

			switch (functionCode)
			{
				case Modbus.ReadCoils:
				case Modbus.ReadInputs:
				case Modbus.ReadHoldingRegisters:
				case Modbus.ReadInputRegisters:
					numBytes = byteCount + 1;
					break;
				case Modbus.WriteSingleCoil:
				case Modbus.WriteSingleRegister:
				case Modbus.WriteMultipleCoils:
				case Modbus.WriteMultipleRegisters:
					numBytes = 4;
					break;
				default:
					numBytes = 1;
					break;
			}

			return numBytes;
		}		
	}
}
