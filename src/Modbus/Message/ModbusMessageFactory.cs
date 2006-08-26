using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Modbus.Data;
using System.IO.Ports;

namespace Modbus.Message
{
	static class ModbusMessageFactory
	{	
		public static T CreateModbusMessage<T>(byte[] frame) where T : IModbusMessage, new()
		{
			IModbusMessage message = new T();
			message.Initialize(frame);

			return (T) message;
		}
	}
}
