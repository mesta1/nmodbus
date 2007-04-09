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
		[TestFixtureSetUp]
		public override void Init()
		{
			base.Init();

			SlaveTcp = new TcpListener(TcpHost, TcpPort);
			SlaveTcp.Start();
			Slave = ModbusTcpSlave.CreateTcp(SlaveAddress, SlaveTcp);
			StartSlave();

			MasterTcp = new TcpClient(TcpHost.ToString(), TcpPort);
			Master = ModbusTcpMaster.CreateTcp(MasterTcp);
		}

		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			MasterTcp.Close();
			SlaveTcp.Stop();
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
		public void ReadWriteMultipleRegisters()
		{
			ushort startReadAddress = 120;
			ushort numberOfPointsToRead = 5;
			ushort startWriteAddress = 50;			
			ushort[] valuesToWrite = new ushort[] { 10, 20, 30, 40, 50 };

			ushort[] valuesToRead = Master.ReadHoldingRegisters(SlaveAddress, startReadAddress, numberOfPointsToRead);
			ushort[] readValues = Master.ReadWriteMultipleRegisters(SlaveAddress, startReadAddress, numberOfPointsToRead, startWriteAddress, valuesToWrite);
			Assert.AreEqual(valuesToRead, readValues);

			ushort[] writtenValues = Master.ReadHoldingRegisters(SlaveAddress, startWriteAddress, (ushort) valuesToWrite.Length);
			Assert.AreEqual(valuesToWrite, writtenValues);
		}
	}
}
