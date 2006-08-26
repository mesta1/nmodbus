using System;
using System.Collections.Generic;
using System.Text;
using Modbus.IO;
using System.Net.Sockets;

namespace Modbus.Device
{
	/// <summary>
	/// Modbus IP based TCP slave.	
	/// </summary>
	public class ModbusTcpSlave : ModbusSlave		
	{
		public ModbusTcpSlave(byte unitID, Socket socket)
			: base(unitID, new ModbusTcpTransport(socket))
		{
		}
	}
}
