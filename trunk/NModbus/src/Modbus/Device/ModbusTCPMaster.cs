using System;
using System.Collections.Generic;
using System.Text;
using Modbus.IO;
using System.Net.Sockets;

namespace Modbus.Device
{
	/// <summary>
	/// Modbus IP based TCP master.
	/// </summary>
	public class ModbusTCPMaster : ModbusMaster
	{
		public ModbusTCPMaster(Socket socket)
			: base(new ModbusTCPTransport(socket))
		{
		}
	}
}
