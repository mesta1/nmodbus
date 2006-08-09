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
				byte[] frameBytes = new byte[2];

				SerialPort.Read(frameBytes, 0, 2);

				// check for slave exception response
				if (frameBytes[1] > Modbus.ExceptionOffset)
				{
					//ModbusMessageFactory.CreateModbusMessage<SlaveExceptionResponse>(frameBytes)
					throw new SlaveException();
				}

				return default(T);
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

		public IModbusMessage ReadResponse(string message, ushort dataLength)
		{
			return null;
		}
	}
}
