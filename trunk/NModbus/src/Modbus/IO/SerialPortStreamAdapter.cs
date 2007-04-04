using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

namespace Modbus.IO
{
	public class SerialPortStreamAdapter
	{
		private SerialPort _serialPort;

		public SerialPortStreamAdapter()
		{
		}		

		public SerialPortStreamAdapter(SerialPort serialPort)
		{
			_serialPort = serialPort;
		}

		public virtual int ReadTimeout
		{
			get { return _serialPort.ReadTimeout; }
			set { _serialPort.ReadTimeout = value; }
		}
		
		public virtual int WriteTimeout
		{
			get { return _serialPort.WriteTimeout; }
			set { _serialPort.WriteTimeout = value; }
		}

		public void DiscardInBuffer()
		{
			_serialPort.DiscardInBuffer();
		}

		public virtual int Read(byte[] buffer, int offset, int count)
		{
			return _serialPort.Read(buffer, offset, count);
		}

		public virtual string ReadLine()
		{
			return _serialPort.ReadLine();
		}

		public virtual void Write(byte[] buffer, int offset, int count)
		{
			_serialPort.Write(buffer, offset, count);
		}		
	}
}
