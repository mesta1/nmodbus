using System;
using System.Collections.Generic;
using System.Text;

namespace Modbus.Message
{
	/// <summary>
	/// Common to all modbus messages.
	/// </summary>
	public interface IModbusMessage
	{
		void Initialize(byte[] frame);
		byte[] ProtocolDataUnit { get; }
		byte[] ChecksumBody { get; }
		byte FunctionCode { get; set; }
		byte SlaveAddress { get; set; }
	}
}
