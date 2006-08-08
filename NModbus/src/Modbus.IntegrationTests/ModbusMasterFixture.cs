using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Device;
using System.IO.Ports;

namespace Modbus.IntegrationTests
{
	// NOTE: this integration test requires a Modbus Slave running at the configuration below
	[TestFixture]
	public class ModbusMasterFixture
	{
		private SerialPort _port;
		private ModbusMaster _master;

		private const string PortName = "COM4";
		private const byte SlaveAddress = 2;

		[TestFixtureSetUp]
		public void Init()
		{
			_port = new SerialPort(PortName);
			_port.ReadTimeout = Modbus.DefaultTimeout;
			_port.Parity = Parity.None;
			_port.Open();
			_master = new ModbusASCIIMaster(_port);
		}

		[TestFixtureTearDown]
		public void Dispose()
		{
			_port.Close();
			_port.Dispose();
		}

		[Test]
		public void ReadCoils()
		{
			bool[] coils = _master.ReadCoils(SlaveAddress, 100, 1);
			Assert.AreEqual(new bool[] { true }, coils);
		}

		[Test]
		public void ReadInputs()
		{
			bool[] inputs = _master.ReadInputs(SlaveAddress, 150, 3);
			Assert.AreEqual(new bool[] { true, true, true }, inputs);
		}

		[Test]
		public void ReadHoldingRegisters()
		{
			ushort[] registers = _master.ReadHoldingRegisters(SlaveAddress, 104, 2);
			Assert.AreEqual(new ushort[] { 104, 105 }, registers);
		}

		[Test]
		public void WriteSingleCoil()
		{
			bool coilValue = _master.ReadCoils(SlaveAddress, 105, 1)[0];
			_master.WriteSingleCoil(SlaveAddress, 105, !coilValue);
			Assert.AreEqual(!coilValue, _master.ReadCoils(SlaveAddress, 105, 1)[0]);
			_master.WriteSingleCoil(SlaveAddress, 105, coilValue);
			Assert.AreEqual(coilValue, _master.ReadCoils(SlaveAddress, 105, 1)[0]);
		}

		[Test]
		public void WriteSingleRegister()
		{
			ushort testAddress = 200;
			ushort testValue = 350;

			ushort originalValue = _master.ReadHoldingRegisters(SlaveAddress, testAddress, 1)[0];
			_master.WriteSingleRegister(SlaveAddress, testAddress, testValue);
			Assert.AreEqual(testValue, _master.ReadHoldingRegisters(SlaveAddress, testAddress, 1)[0]);
			_master.WriteSingleRegister(SlaveAddress, testAddress, originalValue);
			Assert.AreEqual(originalValue, _master.ReadHoldingRegisters(SlaveAddress, testAddress, 1)[0]);
		}

		[Test]
		public void WriteMultipleRegisters()
		{
			ushort testAddress = 200;
			ushort[] testValues = new ushort[] { 10, 20, 30, 40, 50 };

			ushort[] originalValues = _master.ReadHoldingRegisters(SlaveAddress, testAddress, (ushort)testValues.Length);
			_master.WriteMultipleRegisters(SlaveAddress, testAddress, testValues);
			ushort[] newValues = _master.ReadHoldingRegisters(SlaveAddress, testAddress, (ushort)testValues.Length);
			Assert.AreEqual(testValues, newValues);
			_master.WriteMultipleRegisters(SlaveAddress, testAddress, originalValues);
		}

		[Test]
		public void WriteMultipleCoils()
		{
			ushort testAddress = 200;
			bool[] testValues = new bool[] { true, false, true, false, false, false, true, false, true, false };

			bool[] originalValues = _master.ReadCoils(SlaveAddress, testAddress, (ushort)testValues.Length);
			_master.WriteMultipleCoils(SlaveAddress, testAddress, testValues);
			bool[] newValues = _master.ReadCoils(SlaveAddress, testAddress, (ushort)testValues.Length);
			Assert.AreEqual(testValues, newValues);
			_master.WriteMultipleCoils(SlaveAddress, testAddress, originalValues);
		}

		[Test]
		[ExpectedException(typeof(SlaveException))]
		public void SlaveException()
		{
			_master.ReadCoils(SlaveAddress, 650, 1);
			Assert.Fail();
		}
	}
}
