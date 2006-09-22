using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO.Ports;
using Modbus.Device;
using System.Threading;

namespace Modbus.IntegrationTests
{
	[TestFixture]
	public class ModbusAsciiMasterFixture : ModbusMasterFixture
	{
		[TestFixtureSetUp]
		public override void Init()
		{
			base.Init();
			Master = ModbusSerialMaster.CreateAscii(MasterPort);
			MasterPort.ReadTimeout = 1000;
			Master.Transport.Retries = 10;
			//Slave = ModbusSlave.CreateAscii(SlaveAddress, SlavePort);

			//Thread thread = new Thread(new ThreadStart(Slave.Listen));
			//thread.Start();
		}

		[Test]
		public override void ReadCoils()
		{ 
			base.ReadCoils();
			string leftInBuffer =  MasterPort.ReadLine();
		}

		[Test]
		public override void Read0Coils()
		{
			base.Read0Coils();			
		}

		[Test]
		public override void ReadInputs()
		{
			base.ReadInputs();
		}

		[Test]
		public override void ReadHoldingRegisters()
		{
			base.ReadHoldingRegisters();
		}

		[Test]
		public override void ReadInputRegisters()
		{
			base.ReadInputRegisters();
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
	}
}
