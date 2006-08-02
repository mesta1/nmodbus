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
		private const string PortName = "COM4";
		private const byte SlaveAddress = 2;

		[SetUp]
		public void SetUp()
		{
			_port = new SerialPort(PortName);
			_port.ReadTimeout = Modbus.DefaultTimeout;
			_port.Parity = Parity.None;
			_port.Open();
		}

		[TearDown]
		public void TearDown()
		{
			_port.Close();
			_port.Dispose();
		}

		[Test]
		public void CheckReadCoils()
		{
			ModbusASCIIMaster master = new ModbusASCIIMaster(_port);
			bool[] coils = master.ReadCoils(SlaveAddress, 100, 1);
			Assert.AreEqual(new bool[] { true }, coils);
		}

		[Test]
		public void CheckReadHoldingRegisters()
		{
			ModbusASCIIMaster master = new ModbusASCIIMaster(_port);
			ushort[] registers = master.ReadHoldingRegisters(SlaveAddress, 104, 2);
			Assert.AreEqual(new ushort[] { 104, 105 }, registers);
		}

		[Test]
		public void CheckWriteSingleCoil()
		{
			ModbusASCIIMaster master = new ModbusASCIIMaster(_port);

			bool coilValue = master.ReadCoils(SlaveAddress, 105, 1)[0];
			master.WriteSingleCoil(SlaveAddress, 105, !coilValue);
			Assert.AreEqual(!coilValue, master.ReadCoils(SlaveAddress, 105, 1)[0]);
			master.WriteSingleCoil(SlaveAddress, 105, coilValue);
			Assert.AreEqual(coilValue, master.ReadCoils(SlaveAddress, 105, 1)[0]);
		}

		//[Test]
		//public void CheckWriteSingleRegister()
		//{
		//    ModbusASCIIMaster master = new ModbusASCIIMaster(_port);

		//    ushort registerValue = master.ReadHoldingRegisters(SlaveAddress, 100, 1)[0];
		//}
	}
}
