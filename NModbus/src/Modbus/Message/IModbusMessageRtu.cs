using System;

namespace Modbus.Message
{
	/// <summary>
	/// Extensions to the IModbusMessage interface to support custom messages over the RTU protocol.
	/// </summary>
	public interface IModbusMessageRtu : IModbusMessage
	{
		/// <summary>
		/// Gets the remaining length of the response given the specified frame start.
		/// </summary>
		Func<byte[], int> RtuResponseBytesRemaining { get; }

		/// <summary>
		/// Gets the remaining length of the request given the specified frame start.
		/// </summary>
		Func<byte[], int> RtuRequestBytesRemaining { get; }
	}
}
