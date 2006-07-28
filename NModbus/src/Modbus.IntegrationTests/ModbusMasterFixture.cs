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

		[SetUp]
		public void SetUp()
		{
			_port = new SerialPort(PortName);
			_port.ReadTimeout = Modbus.DEFAULT_TIMEOUT;
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
			bool[] coils = master.ReadCoils(2, 100, 1);
			Assert.AreEqual(new bool[] { true }, coils);
		}

		[Test]
		public void CheckReadHoldingRegisters()
		{
			ModbusASCIIMaster master = new ModbusASCIIMaster(_port);
			ushort[] registers = master.ReadHoldingRegisters(2, 104, 2);
			Assert.AreEqual(new ushort[] { 104, 105 }, registers);
		}
	}
}
