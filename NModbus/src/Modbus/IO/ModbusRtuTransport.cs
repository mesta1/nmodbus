using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Message;
using System.IO.Ports;
using Modbus.Util;
using System.IO;
using Modbus.IO;
using log4net;

namespace Modbus.IO
{
	class ModbusRtuTransport : ModbusSerialTransport
	{		
		public const int RequestFrameStartLength = 7;
		public const int ResponseFrameStartLength = 4;

		private static readonly ILog _log = LogManager.GetLogger(typeof(ModbusRtuTransport));

		public ModbusRtuTransport ()
		{
		}

		public ModbusRtuTransport(SerialPort serialPort)
			: base (serialPort)
		{
		}

		internal override byte[] BuildMessageFrame(IModbusMessage message)
		{
			List<byte> messageBody = new List<byte>();
			messageBody.Add(message.SlaveAddress);
			messageBody.AddRange(message.ProtocolDataUnit);
			messageBody.AddRange(ModbusUtil.CalculateCrc(message.MessageFrame));

			return messageBody.ToArray();
		}

		internal override bool ChecksumsMatch(IModbusMessage message, byte[] messageFrame)
		{
			return BitConverter.ToUInt16(messageFrame, messageFrame.Length - 2) == BitConverter.ToUInt16(ModbusUtil.CalculateCrc(message.MessageFrame), 0);
		}		
		
		internal override byte[] ReadResponse()
		{
			byte[] frameStart = Read(ResponseFrameStartLength);
			_log.DebugFormat("Frame start {0}.", StringUtil.Join(", ", frameStart));	

			byte[] frameEnd = Read(ResponseBytesToRead(frameStart));
			_log.DebugFormat("Frame end {0}.", StringUtil.Join(", ", frameEnd));

			return CollectionUtil.Combine<byte>(frameStart, frameEnd);
		}

		internal override byte[] ReadRequest()
		{			
			byte[] frameStart = Read(RequestFrameStartLength);
			_log.DebugFormat("Frame start {0}.", StringUtil.Join(", ", frameStart));			
			
			byte[] frameEnd = Read(RequestBytesToRead(frameStart));
			_log.DebugFormat("Frame end {0}.", StringUtil.Join(", ", frameEnd));

			return CollectionUtil.Combine<byte>(frameStart, frameEnd);
		}

		public byte[] Read(int count)
		{
			_log.DebugFormat("Read {0} bytes.", count);
			byte[] frameBytes = new byte[count];
			int numBytesRead = 0;			

			while (numBytesRead != count)
				numBytesRead += SerialPort.Read(frameBytes, numBytesRead, count - numBytesRead);			

			return frameBytes;
		}

		public static int RequestBytesToRead(byte[] frameStart)
		{
			byte functionCode = frameStart[1];
			int numBytes;

			switch (functionCode)
			{
				case Modbus.ReadCoils:
				case Modbus.ReadInputs:
				case Modbus.ReadHoldingRegisters:
				case Modbus.ReadInputRegisters:
				case Modbus.WriteSingleCoil:
				case Modbus.WriteSingleRegister:
				case Modbus.Diagnostics:
					numBytes = 1;
					break;
				case Modbus.WriteMultipleCoils:
				case Modbus.WriteMultipleRegisters:
					byte byteCount = frameStart[6];
					numBytes = byteCount + 2;
					break;
				default:
					string errorMessage = String.Format("Function code {0} not supported.", functionCode);
					_log.Error(errorMessage);
					throw new NotImplementedException(errorMessage);
			}

			return numBytes;
		}

		public static int ResponseBytesToRead(byte[] frameStart)
		{
			byte functionCode = frameStart[1];
			byte byteCount = frameStart[2];
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
				case Modbus.Diagnostics:
					numBytes = 4;
					break;
				default:
					string errorMessage = String.Format("Function code {0} not supported.", functionCode);
					_log.Error(errorMessage);
					throw new NotImplementedException(errorMessage);
			}

			return numBytes;
		}		
	}
}
