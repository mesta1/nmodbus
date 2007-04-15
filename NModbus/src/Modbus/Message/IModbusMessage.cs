namespace Modbus.Message
{
	/// <summary>
	/// Common to all modbus messages.
	/// </summary>
	interface IModbusMessage
	{
		void Initialize(byte[] frame);
		byte[] MessageFrame { get; }
		byte[] ProtocolDataUnit { get; }
		ushort TransactionID { get; set; }
		byte FunctionCode { get; set; }
		byte SlaveAddress { get; set; }
	}
}
