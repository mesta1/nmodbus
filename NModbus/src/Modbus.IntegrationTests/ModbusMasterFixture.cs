using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Device;
using System.IO.Ports;

namespace Modbus.IntegrationTests
{
	// NOTE: this integration test requires a Modbus Slave running at the configuration below
	public class ModbusMasterFixture
	{
		public SerialPort Port;
		public ModbusMaster Master;

		public const string PortName = "COM4";
		private const byte SlaveAddress = 2;

		public virtual void Init()
		{
			Port = new SerialPort(PortName);
			Port.ReadTimeout = Modbus.DefaultTimeout;
			Port.Parity = Parity.None;
			Port.Open();
		}

		[TestFixtureTearDown]
		public void Dispose()
		{
			Port.Close();
			Port.Dispose();
		}

		[Test]
		public virtual void ReadCoils()
		{
			bool[] coils = Master.ReadCoils(SlaveAddress, 100, 1);
			Assert.AreEqual(new bool[] { false }, coils);
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
			Assert.AreEqual(new bool[] { true, true, true }, inputs);
		}

		[Test]
		public virtual void ReadHoldingRegisters()
		{
			ushort[] registers = Master.ReadHoldingRegisters(SlaveAddress, 104, 2);
			Assert.AreEqual(new ushort[] { 104, 105 }, registers);
		}

		[Test]
		public virtual void WriteSingleCoil()
		{
			bool coilValue = Master.ReadCoils(SlaveAddress, 105, 1)[0];
			Master.WriteSingleCoil(SlaveAddress, 105, !coilValue);
			Assert.AreEqual(!coilValue, Master.ReadCoils(SlaveAddress, 105, 1)[0]);
			Master.WriteSingleCoil(SlaveAddress, 105, coilValue);
			Assert.AreEqual(coilValue, Master.ReadCoils(SlaveAddress, 105, 1)[0]);
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
			ushort testAddress = 200;
			ushort[] testValues = new ushort[] { 10, 20, 30, 40, 50 };

			ushort[] originalValues = Master.ReadHoldingRegisters(SlaveAddress, testAddress, (ushort)testValues.Length);
			Master.WriteMultipleRegisters(SlaveAddress, testAddress, testValues);
			ushort[] newValues = Master.ReadHoldingRegisters(SlaveAddress, testAddress, (ushort)testValues.Length);
			Assert.AreEqual(testValues, newValues);
			Master.WriteMultipleRegisters(SlaveAddress, testAddress, originalValues);
		}

		[Test]
		public virtual void WriteMultipleCoils()
		{
			ushort testAddress = 200;
			bool[] testValues = new bool[] { true, false, true, false, false, false, true, false, true, false };

			bool[] originalValues = Master.ReadCoils(SlaveAddress, testAddress, (ushort)testValues.Length);
			Master.WriteMultipleCoils(SlaveAddress, testAddress, testValues);
			bool[] newValues = Master.ReadCoils(SlaveAddress, testAddress, (ushort)testValues.Length);
			Assert.AreEqual(testValues, newValues);
			Master.WriteMultipleCoils(SlaveAddress, testAddress, originalValues);
		}

		[Test]
		[ExpectedException(typeof(SlaveException))]
		public virtual void SlaveExceptionMinimumFunctionCode()
		{
			Master.ReadCoils(SlaveAddress, 650, 1);
			Assert.Fail();
		}
	}
}
