using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Device;
using System.Net.Sockets;

namespace Modbus.IntegrationTests
{
	[TestFixture]
	public class ModbusTcpJamodSlaveFixture : ModbusMasterFixture
	{
		private string program = String.Format("TcpSlave {0}", TcpPort);

		[TestFixtureSetUp]
		public override void Init()
		{
			base.Init();

			StartJamodSlave(program);

			MasterTcp = new TcpClient(TcpHost.ToString(), TcpPort);
			Master = ModbusTcpMaster.CreateTcp(MasterTcp);
		}

		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			Jamod.CloseMainWindow();
			Jamod.Close();
			Jamod.Dispose();
		}

		[Test]
		public override void ReadCoils()
		{
			base.ReadCoils();
		}
	}
}
