using System.IO;
using System.IO.Ports;
using System.Reflection;
using log4net;
using Modbus.Message;
using System;

namespace Modbus.IO
{
	abstract class ModbusSerialTransport : ModbusTransport
	{
		private SerialPort _serialPort;
		private TextReader _reader;

		public ModbusSerialTransport()
		{
		}

		public ModbusSerialTransport(SerialPort serialPort)
		{
			if (serialPort == null)
				throw new ArgumentNullException("serialPort");

			_serialPort = serialPort;
			_reader = new StreamReader(_serialPort.BaseStream);
		}	

		public SerialPort SerialPort
		{
			get { return _serialPort;}
			set { _serialPort = value;}
		}

		public TextReader Reader
		{
			get { return _reader; }
			set { _reader = value; }
		}

		public override void Close()
		{
			if (_serialPort.IsOpen)
				_serialPort.Close();
		}
		
		public override void Write(IModbusMessage message)
		{
			byte[] frame = BuildMessageFrame(message);
			SerialPort.Write(frame, 0, frame.Length);
		}
	
		public override T CreateResponse<T>(byte[] frame)
		{
			byte functionCode = frame[1];

			// check for slave exception response
			if (functionCode > Modbus.ExceptionOffset)
				throw new SlaveException(ModbusMessageFactory.CreateModbusMessage<SlaveExceptionResponse>(frame));

			// create message from frame
			T response = ModbusMessageFactory.CreateModbusMessage<T>(frame);

			// compare checksum
			if (!ChecksumsMatch(response, frame))
				throw new IOException("Checksum failed.");

			return response;
		}

		public abstract bool ChecksumsMatch(IModbusMessage message, byte[] messageFrame);
	}
}
