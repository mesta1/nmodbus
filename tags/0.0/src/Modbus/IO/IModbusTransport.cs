using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Message;

namespace Modbus.IO
{
	public interface IModbusTransport
	{
		/// <summary>
		/// Closes the raw input and output streams of this ModbusTransport.
		/// </summary>
		void Close();

		T Read<T>(IModbusMessage request) where T : IModbusMessage, new();

		void Write(IModbusMessage message);

		T UnicastMessage<T>(IModbusMessage message) where T : IModbusMessage, new();

		void BroadcastMessage(IModbusMessage message);
	}
}
