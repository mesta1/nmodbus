using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Net.Sockets;
using System.Text;
using log4net;
using Modbus.Device;
using NUnit.Framework;
using System.Threading;

namespace Modbus.IntegrationTests
{
	public class ModbusMasterFixture
	{
		public const string MasterPortName = "COM5";
		public const string SlavePortName = "COM1";
		public const byte SlaveAddress = 1;
		public IModbusMaster Master;
		public ModbusSlave Slave;
		public SerialPort MasterPort;
		public SerialPort SlavePort;
		public Socket Sock;		
		
		public virtual void Init()
		{
			log4net.Config.XmlConfigurator.Configure();

			MasterPort = new SerialPort(MasterPortName);
			SlavePort = new SerialPort(SlavePortName);
			MasterPort.ReadTimeout = SlavePort.ReadTimeout = 5000;
			MasterPort.Parity = SlavePort.Parity = Parity.None;
			MasterPort.Open();
		}

		[TestFixtureTearDown]
		public void Dispose()
		{
			if (MasterPort != null && MasterPort.IsOpen)
			{
				MasterPort.Close();
				MasterPort.Dispose();
			}

			if (SlavePort != null && SlavePort.IsOpen)
			{
				SlavePort.Close();
				SlavePort.Dispose();
			}

			if (Sock != null && Sock.Connected)
				Sock.Close();
		}

		[Test]
		public virtual void ReadCoils()
		{
			bool[] coils = Master.ReadCoils(SlaveAddress, 1, 8);
			Assert.AreEqual(new bool[] { false, false, false, false, false, false, false, false }, coils);
		}

		[Test]
		public virtual void Read0Coils()
		{
			bool[] coils = Master.ReadCoils(SlaveAddress, 100, 0);
			Assert.AreEqual(new bool[] { }, coils);
		}

		[Test]
		public virtual void ReadInputs()
		{
			bool[] inputs = Master.ReadInputs(SlaveAddress, 150, 3);
			Assert.AreEqual(new bool[] { false, false, false }, inputs);
		}

		[Test]
		public virtual void ReadHoldingRegisters()
		{
			ushort[] registers = Master.ReadHoldingRegisters(SlaveAddress, 104, 2);
			Assert.AreEqual(new ushort[] { 0, 0 }, registers);
		}

		[Test]
		public virtual void ReadInputRegisters()
		{
			ushort[] registers = Master.ReadInputRegisters(SlaveAddress, 104, 2);
			Assert.AreEqual(new ushort[] { 0, 0 }, registers);
		}

		[Test]
		public virtual void WriteSingleCoil()
		{
			bool coilValue = Master.ReadCoils(SlaveAddress, 10, 1)[0];
			Master.WriteSingleCoil(SlaveAddress, 10, !coilValue);
			Assert.AreEqual(!coilValue, Master.ReadCoils(SlaveAddress, 10, 1)[0]);
			Master.WriteSingleCoil(SlaveAddress, 10, coilValue);
			Assert.AreEqual(coilValue, Master.ReadCoils(SlaveAddress, 10, 1)[0]);
		}

		[Test]
		public virtual void WriteSingleRegister()
		{
			ushort testAddress = 200;
			ushort testValue = 350;

			ushort originalValue = Master.ReadHoldingRegisters(SlaveAddress, testAddress, 1)[0];
			Master.WriteSingleRegister(SlaveAddress, testAddress, testValue);
			Assert.AreEqual(testValue, Master.ReadHoldingRegisters(SlaveAddress, testAddress, 1)[0]);
			Master.WriteSingleRegister(SlaveAddress, testAddress, originalValue);
			Assert.AreEqual(originalValue, Master.ReadHoldingRegisters(SlaveAddress, testAddress, 1)[0]);
		}

		[Test]
		public virtual void WriteMultipleRegisters()
		{
			ushort testAddress = 120;
			ushort[] testValues = new ushort[] { 10, 20, 30, 40, 50 };

			ushort[] originalValues = Master.ReadHoldingRegisters(SlaveAddress, testAddress, (ushort) testValues.Length);
			Master.WriteMultipleRegisters(SlaveAddress, testAddress, testValues);
			ushort[] newValues = Master.ReadHoldingRegisters(SlaveAddress, testAddress, (ushort) testValues.Length);
			Assert.AreEqual(testValues, newValues);
			Master.WriteMultipleRegisters(SlaveAddress, testAddress, originalValues);
		}

		[Test]
		public virtual void WriteMultipleCoils()
		{
			ushort testAddress = 200;
			bool[] testValues = new bool[] { true, false, true, false, false, false, true, false, true, false };

			bool[] originalValues = Master.ReadCoils(SlaveAddress, testAddress, (ushort) testValues.Length);
			Master.WriteMultipleCoils(SlaveAddress, testAddress, testValues);
			bool[] newValues = Master.ReadCoils(SlaveAddress, testAddress, (ushort) testValues.Length);
			Assert.AreEqual(testValues, newValues);
			Master.WriteMultipleCoils(SlaveAddress, testAddress, originalValues);
		}

		// TODO how can we test a slave exception?
	}
}
