using System;
using System.Net.Sockets;
using Modbus.Device;
using NUnit.Framework;

namespace Modbus.IntegrationTests
{
	[TestFixture]
	public class ModbusTcpJamodSlaveFixture : ModbusMasterFixture
	{
		private string program = String.Format("TcpSlave {0}", Port);

		[TestFixtureSetUp]
		public override void Init()
		{
			base.Init();

			StartJamodSlave(program);

			MasterTcp = new TcpClient(TcpHost.ToString(), Port);
			Master = ModbusIpMaster.CreateTcp(MasterTcp);
		}

		[Test]
		public override void ReadCoils()
		{
			base.ReadCoils();
		}
	}
}
