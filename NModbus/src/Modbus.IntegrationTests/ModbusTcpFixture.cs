using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Device;
using System.Net.Sockets;
using System.Net;

namespace Modbus.IntegrationTests
{
	[TestFixture]
	public class ModbusTcpFixture : ModbusMasterFixture
	{
		public const string TcpClientHost = "127.0.0.1";
		public const int TcpClientPort = 502;
		public TcpClient MasterTcp;
		public TcpListener SlaveTcp;

		
		[TestFixtureSetUp]
		public override void Init()
		{
			SlaveTcp = new TcpListener(new IPAddress(new byte[] { 127, 0, 0, 1 }), TcpClientPort);
			Slave = ModbusTcpSlave.CreateTcp(SlaveAddress, SlaveTcp);
			MasterTcp = new TcpClient(TcpClientHost, TcpClientPort);
			Master = ModbusTcpMaster.CreateTcp(MasterTcp);
		}

		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			// TODO expose close...
			//MasterSocket.Close();
			//SlaveSocket.Close();
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
			// test fails, bug in slave device MOD_RSsim version 7.5?
			base.WriteSingleRegister();
		}

		[Test]
		public override void WriteMultipleRegisters()
		{
			base.WriteMultipleRegisters();
		}
	}
}
