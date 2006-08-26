using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Modbus.Data
{
	interface IModbusMessageDataCollection	
	{
		byte[] NetworkBytes { get; }
	}
}
