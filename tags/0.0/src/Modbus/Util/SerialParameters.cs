using System;
using System.Collections.Generic;
using System.IO.Ports;


namespace Modbus.Util
{
	public class SerialParameters
	{
		private string _portName;
		private int _baudRate;
		private Parity _parity;
		private int _databits;
		private StopBits _stopbits;
		// TODO handshake?
		//private Handshake _handshake;

		public string PortName
		{
			get { return _portName; }
			set { _portName = value; }
		}

		public int BaudRate
		{
			get { return _baudRate; }
			set { _baudRate = value; }
		}

		public Parity Parity
		{
			get { return _parity; }
			set { _parity = value; }
		}

		public int DataBits
		{
			get { return _databits; }
			set { _databits = value; }
		}

		public StopBits StopBits
		{
			get { return _stopbits; }
			set { _stopbits = value; }
		}
				
		/// <summary>
		/// Constructs SerialParameters instance with default values.
		/// </summary>
		public SerialParameters()
		{	
			_portName = "COM1";
			_baudRate = 9600;
			_databits = 8;
			_stopbits = StopBits.One;
			_parity = Parity.None;			
		}

		/// <summary>
		/// Constructs SerialParameters instance with given parameters;
		/// </summary>
		public SerialParameters(string portName, int baudRate, int databits, StopBits stopbits, Parity parity)
		{
			_portName = portName;
			_baudRate = baudRate;
			_databits = databits;
			_stopbits = stopbits;
			_parity = parity;
		}
	}
}
