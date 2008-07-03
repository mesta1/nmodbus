using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using System.IO.Ports;
using Modbus.Device;
using Modbus.Data;
using System.Threading;

namespace Modbus.IntegrationTests
{
	[TestFixture]
	public class NModbusSerialRtuSlaveFixture
	{
		[TestFixtureSetUp]
		public void SetUp()
		{
			log4net.Config.XmlConfigurator.Configure();
		}

		[Test]
		public void NModbusSerialRtuSlave_BonusCharacter_VerifyTimeout()
		{
			using (SerialPort masterPort = ModbusMasterFixture.CreateAndOpenSerialPort(ModbusMasterFixture.DefaultMasterSerialPortName))
			using (SerialPort slavePort = ModbusMasterFixture.CreateAndOpenSerialPort(ModbusMasterFixture.DefaultSlaveSerialPortName))
			{
				IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(masterPort);

				ModbusSerialSlave slave = ModbusSerialSlave.CreateRtu(1, slavePort);
				slave.DataStore = DataStoreFactory.CreateTestDataStore();

				Thread slaveThread = new Thread(slave.Listen);
				slaveThread.IsBackground = true;
				slaveThread.Start();

				// assert successful communication
				Assert.AreEqual(new bool[] { false, true }, master.ReadCoils(1, 1, 2));

				// write "bonus" character
				masterPort.Write("*");

				// assert successful communication
				Assert.AreEqual(new bool[] { false, true }, master.ReadCoils(1, 1, 2));
			}
		}
	}
}
