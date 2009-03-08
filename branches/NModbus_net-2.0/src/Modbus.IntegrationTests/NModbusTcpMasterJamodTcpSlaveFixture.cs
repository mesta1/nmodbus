using System;
using System.Globalization;
using System.Net.Sockets;
using MbUnit.Framework;
using Modbus.Device;

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
			Master = ModbusIpMaster.CreateIp(MasterTcp);
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
