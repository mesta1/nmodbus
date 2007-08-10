using System.Net.Sockets;
using Modbus.Device;
using NUnit.Framework;
using System.Threading;

namespace Modbus.IntegrationTests
{
	[TestFixture]
	public class ModbusTcpMasterFixture : ModbusMasterFixture
	{
		[TestFixtureSetUp]
		public override void Init()
		{
			base.Init();

			SlaveTcp = new TcpListener(TcpHost, Port);
			SlaveTcp.Start();
			Slave = ModbusTcpSlave.CreateTcp(SlaveAddress, SlaveTcp);
			StartSlave();

			MasterTcp = new TcpClient(TcpHost.ToString(), Port);
			Master = ModbusIpMaster.CreateTcp(MasterTcp);
			Master.Transport.Retries = 0;
		}

		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			SlaveTcp.Stop();
			MasterTcp.Close();
			CleanUp();
		}

		[Test]
		public override void ReadCoils()
		{
			base.ReadCoils();
		}

		[Test]
		public override void Read0Coils()
		{
			base.Read0Coils();
		}

		[Test]
		public override void ReadHoldingRegisters()
		{
			base.ReadHoldingRegisters();
		}

		[Test]
		public override void ReadInputs()
		{
			base.ReadInputs();
		}

		[Test]
		public override void WriteSingleCoil()
		{
			base.WriteSingleCoil();
		}

		[Test]
		public override void WriteMultipleCoils()
		{
			base.WriteMultipleCoils();
		}

		[Test]
		public override void WriteSingleRegister()
		{
			base.WriteSingleRegister();
		}

		[Test]
		public override void WriteMultipleRegisters()
		{
			base.WriteMultipleRegisters();
		}

		[Test]
		public override void ReadMaximumNumberOfHoldingRegisters()
		{
			base.ReadMaximumNumberOfHoldingRegisters();
		}

		[Test]
		public override void ReadWriteMultipleRegisters()
		{
			base.ReadWriteMultipleRegisters();
		}
	}
}
