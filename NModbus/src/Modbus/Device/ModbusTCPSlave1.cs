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
	public class ModbusTCPSlave1 : ModbusSlave		
	{
		public ModbusTCPSlave1(byte unitID, Socket socket)
			: base(unitID, new ModbusTCPTransport1(socket))
		{
		}
	}
}
