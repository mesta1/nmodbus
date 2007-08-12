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

			Slave = ModbusUdpSlave.CreateUdp(SlaveAddress, new UdpClient(Port), DefaultModbusIPEndPoint);
			StartSlave();

			MasterUdp = new UdpClient();
			MasterUdp.Connect(DefaultModbusIPEndPoint);
			Master = ModbusIpMaster.CreateUdp(MasterUdp, DefaultModbusIPEndPoint);
			Master.Transport.Retries = 0;
		}

		[Test]
		public override void ReadCoils()
		{
			base.ReadCoils();
		}

		[Test]
		public override void SimpleReadRegistersPerformanceTest()
		{
			base.SimpleReadRegistersPerformanceTest();
		}
	}
}
