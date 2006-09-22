using System.IO;
using System.IO.Ports;
using System.Reflection;
using Modbus.Message;
using System;

namespace Modbus.IO
{
	public abstract class ModbusSerialTransport : ModbusTransport
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

		internal SerialPort SerialPort
		{
			get { return _serialPort;}
			set { _serialPort = value;}
		}

		internal TextReader Reader
		{
			get { return _reader; }
			set { _reader = value; }
		}

		internal override void Write(IModbusMessage message)
		{
			byte[] frame = BuildMessageFrame(message);
			SerialPort.Write(frame, 0, frame.Length);
		}

		internal override T UnicastMessage<T>(IModbusMessage message)
		{
			// clear any old messages from input buffer
			_serialPort.DiscardInBuffer();

			return base.UnicastMessage<T>(message);
		}

		internal override T CreateResponse<T>(byte[] frame)
		{
			T response = base.CreateResponse<T>(frame);

			// compare checksum
			if (!ChecksumsMatch(response, frame))
				throw new IOException("Checksum failed.");

			return response;
		}

		internal abstract bool ChecksumsMatch(IModbusMessage message, byte[] messageFrame);
	}
}
