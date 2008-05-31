using System;
using System.Net.Sockets;
using Modbus.Device;
using NUnit.Framework;
using System.Globalization;

namespace Modbus.IntegrationTests
{
	[TestFixture]
	public class NModbusTcpMasterJamodTcpSlaveFixture : ModbusMasterFixture
	{
		private string program = String.Format(CultureInfo.InvariantCulture, "TcpSlave {0}", Port);

		[TestFixtureSetUp]
		public override void Init()
		{
			base.Init();

			StartJamodSlave(program);

			MasterTcp = new TcpClient(TcpHost.ToString(), Port);
			Master = ModbusIpMaster.CreateTcp(MasterTcp);
		}

		/// <summary>
		/// Not supported by the Jamod Slave
		/// </summary>
		public override void ReadWriteMultipleRegisters()
		{
		}

		[Test]
		public override void ReadCoils()
		{
			base.ReadCoils();
		}
	}
}
