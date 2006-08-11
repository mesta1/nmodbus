using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Message;
using System.IO.Ports;
using Modbus.Util;
using System.IO;

namespace Modbus.IO
{
	public class ModbusRTUTransport : ModbusSerialTransport, IModbusTransport
	{
		public ModbusRTUTransport(SerialPort serialPort)
			: base (serialPort)
		{
		}

		public override byte[] BuildMessageFrame(IModbusMessage message)
		{
			List<byte> messageBody = new List<byte>();
			messageBody.Add(message.SlaveAddress);
			messageBody.AddRange(message.ProtocolDataUnit);
			messageBody.AddRange(ModbusUtil.CalculateCRC(message.ChecksumBody));

			return messageBody.ToArray();
		}

		public override T Read<T>(IModbusMessage request)
		{
			try
			{	
				byte[] frameStart = new byte[4];
				SerialPort.Read(frameStart, 0, 4);

				int bytesRemaining = NumberOfBytesToRead(frameStart[1], frameStart[2], frameStart[3]);
				byte[] frameEnd = new byte[bytesRemaining];
				SerialPort.Read(frameEnd, 0, bytesRemaining);

				byte[] frame = CollectionUtil.Combine<byte>(frameStart, frameEnd);

				// check for slave exception response
				if (frameStart[1] > Modbus.ExceptionOffset)
					throw new SlaveException(ModbusMessageFactory.CreateModbusMessage<SlaveExceptionResponse>(frame));

				T response = ModbusMessageFactory.CreateModbusMessage<T>(frame);

				return response;
			}
			catch (TimeoutException te)
			{
				throw te;
			}
			catch (IOException ioe)
			{

				throw ioe;
			}
		}

		public static int NumberOfBytesToRead(byte functionCode, byte byteCount1, byte byteCount2)
		{
			int numBytes;

			switch(functionCode)
			{
				case Modbus.ReadCoils:
				case Modbus.ReadInputs:
				case Modbus.ReadHoldingRegisters:
				case Modbus.ReadInputRegisters:
					numBytes = byteCount1 - 1;
					break;
				case Modbus.WriteMultipleCoils:
				case Modbus.WriteMultipleRegisters:
					numBytes = 2;
					break;
				default :
					numBytes = -1;
					break;
			}

			// CRC
			numBytes += 2;

			return numBytes;
		}

		public IModbusMessage ReadResponse(string message, ushort dataLength)
		{
			return null;
		}
	}
}
