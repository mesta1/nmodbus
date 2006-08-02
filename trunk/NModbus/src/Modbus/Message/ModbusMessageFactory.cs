using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Modbus.Data;

namespace Modbus.Message
{
	public static class ModbusMessageFactory
	{	
		public static T CreateModbusMessage<T>(byte[] frame) where T : IModbusMessage, new()
		{
			IModbusMessage message = new T();
			message.Initialize(frame);

			return (T) message;
		}
	}
}
