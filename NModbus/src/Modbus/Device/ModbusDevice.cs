using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Modbus.IO;

namespace Modbus.Device
{
	public abstract class ModbusDevice
	{
		private ModbusTransport _transport;

		internal ModbusTransport Transport
		{
			get { return _transport; }
			set { _transport = value; }
		}

		internal ModbusDevice(ModbusTransport transport)
		{
			_transport = transport;
		}
	}
}
