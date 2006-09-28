using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Device;
using System.Net.Sockets;

namespace Modbus.IntegrationTests
{
	[TestFixture]
	public class ModbusTcpFixture : ModbusMasterFixture
	{
		public const string SocketHost = "127.0.0.1";
		public const int SocketPort = 502;
		public Socket SlaveSocket;
		public Socket MasterSocket;

		[TestFixtureSetUp]
		public override void Init()
		{
			Socket SlaveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			SlaveSocket.Connect(SocketHost, SocketPort);
			Slave = ModbusSlave.CreateTcp(SlaveAddress, SlaveSocket);

			Socket MasterSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			MasterSocket.Connect(SocketHost, SocketPort);
			Master = ModbusTcpMaster.CreateTcp(MasterSocket);
		}

		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			MasterSocket.Close();
			SlaveSocket.Close();
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
