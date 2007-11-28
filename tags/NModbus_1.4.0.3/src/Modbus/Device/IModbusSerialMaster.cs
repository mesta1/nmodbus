using Modbus.IO;

namespace Modbus.Device
{
	/// <summary>
	/// Modbus Serial Master device.
	/// </summary>
	public interface IModbusSerialMaster : IModbusMaster
	{
		/// <summary>
		/// Transport for used by this master.
		/// </summary>
		new ModbusSerialTransport Transport { get; }
	}
}
