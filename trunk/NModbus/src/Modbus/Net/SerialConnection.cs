using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using Modbus.Util;
using System.IO;


namespace Modbus.Net
{
	public class SerialConnection
	{
		private SerialPort _port;
		
		public bool IsOpen
		{
			get { return _port.IsOpen; }
		}
	
		public SerialConnection(SerialParameters serialParams)
		{
			_port = new SerialPort(serialParams.PortName, serialParams.BaudRate, serialParams.Parity, serialParams.DataBits, serialParams.StopBits);
		}

		public void Open()
		{
			// TODO exception handling
			try
			{
				_port.Open();
			}
			catch (InvalidOperationException)
			{
			}
			catch (ArgumentOutOfRangeException)
			{
			}
			catch (ArithmeticException)
			{
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}			
		}

		public void Close()
		{
			_port.Close();
		}
	}
}
