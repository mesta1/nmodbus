using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Modbus.Device
{
	public class ModbusTcpSlave : ModbusSlave
	{
		private TcpListener tcpListener;

		private ModbusTcpSlave(byte unitID, IPAddress ip, int port)
			: base(unitID)
		{
			tcpListener = new TcpListener(ip, port);
		}

		public static ModbusTcpSlave CreateTcp(byte unitID, IPAddress ip, int port)
		{
			return new ModbusTcpSlave(unitID, ip, port);
		}

		public override void  Listen()
		{
			tcpListener.Start();

			while (true)
			{
				//TcpClient master = tcpListener.AcceptTcpClient();
				//NetworkStream masterStream = client.GetStream();

				//tcpListener.Server
				//masterStream.Read()

			}
		}
	}
}
