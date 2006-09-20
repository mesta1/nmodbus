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

		public ModbusTransport Transport
		{
			get { return _transport; }
		}

		internal ModbusDevice(ModbusTransport transport)
		{
			_transport = transport;
		}
	}
}
