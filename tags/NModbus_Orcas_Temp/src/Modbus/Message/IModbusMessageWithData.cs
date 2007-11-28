namespace Modbus.Message
{
	/// <summary>
	/// A Modbus message containing variably sized data property.
	/// </summary>
	public interface IModbusMessageWithData<TData> : IModbusMessage 
	{
		///<summary>
		/// The message data.
		///</summary>
		TData[] Data { get; }
	}
}
