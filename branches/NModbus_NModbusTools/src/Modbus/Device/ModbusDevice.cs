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
	}
}
