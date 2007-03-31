using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

namespace Modbus.IO
{
	internal class SerialPortTransportAdapter
	{
		private SerialPort _serialPort;

		public SerialPortTransportAdapter(SerialPort serialPort)
		{
			_serialPort = serialPort;
		}

		public void Write(byte[] buffer, int offset, int count)
		{
			_serialPort.Write(buffer, offset, count);
		}

		public int Read(byte[] buffer, int offset, int count)
		{
			return _serialPort.Read(buffer, offset, count);
		}
	}
}
