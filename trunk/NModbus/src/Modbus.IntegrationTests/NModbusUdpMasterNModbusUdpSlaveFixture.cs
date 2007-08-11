using System.Net;
using System.Net.Sockets;
using Modbus.Device;
using NUnit.Framework;

namespace Modbus.IntegrationTests
{
	[TestFixture]
	public class NModbusUdpMasterNModbusUdpSlaveFixture : ModbusMasterFixture
	{
		[TestFixtureSetUp]
		public override void Init()
		{
			base.Init();

			Slave = ModbusUdpSlave.CreateUdp(SlaveAddress, new UdpClient(Port), ModbusIPEndPoint);
			StartSlave();

			MasterUdp = new UdpClient();
			MasterUdp.Connect(ModbusIPEndPoint);
			Master = ModbusIpMaster.CreateUdp(MasterUdp, ModbusIPEndPoint);
			Master.Transport.Retries = 0;
		}

		[Test]
		public override void ReadCoils()
		{
			base.ReadCoils();
		}
	}
}
