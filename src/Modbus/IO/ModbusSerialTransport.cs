using System.IO;
using System.IO.Ports;
using System.Reflection;
using Modbus.Message;
using System;

namespace Modbus.IO
{
	abstract class ModbusSerialTransport : ModbusTransport
	{
		private SerialPort _serialPort;

		public ModbusSerialTransport()
		{
		}

		public ModbusSerialTransport(SerialPort serialPort)
		{
			if (serialPort == null)
				throw new ArgumentNullException("serialPort");

			_serialPort = serialPort;	
		}	

		public SerialPort SerialPort
		{
			get { return _serialPort;}
			set { _serialPort = value;}
		}

		public override void Close()
		{
			_serialPort.Close();
		}
		
		public override void Write(IModbusMessage message)
		{
			byte[] frame = BuildMessageFrame(message);
			SerialPort.Write(frame, 0, frame.Length);
		}
	
		public override abstract T Read<T>(IModbusMessage request);
	}
}
