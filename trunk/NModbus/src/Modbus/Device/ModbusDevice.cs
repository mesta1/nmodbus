using Modbus.IO;
using System;
using Unme.Common;

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
		/// Gets the Modbus Transport.
		/// </summary>
		/// <value>The transport.</value>
		public ModbusTransport Transport
		{
			get 
			{ 
				return _transport; 
			}
			internal set
			{
				_transport = value;
			}
		}

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                DisposableUtility.Dispose(ref _transport);
        }
	}
}
