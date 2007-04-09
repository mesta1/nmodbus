using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Modbus.IO;

namespace Modbus.Device
{
	/// <summary>
	/// Modbus device.
	/// </summary>
	public abstract class ModbusDevice
	{
		private ModbusTransport _transport;

		internal ModbusDevice(ModbusTransport transport)
		{
			_transport = transport;
		}

		/// <summary>
		/// Gets the modbus transport.
		/// </summary>
		public ModbusTransport Transport
		{
			get { return _transport; }
		}
	}
}
