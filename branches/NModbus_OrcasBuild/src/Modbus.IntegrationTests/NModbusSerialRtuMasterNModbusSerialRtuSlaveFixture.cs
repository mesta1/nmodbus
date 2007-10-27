using Modbus.Device;
using NUnit.Framework;

namespace Modbus.IntegrationTests
{
	[TestFixture]
	public class NModbusSerialRtuMasterNModbusSerialRtuSlaveFixture : ModbusSerialMasterFixture
	{
		[TestFixtureSetUp]
		public override void Init()
		{
			base.Init();

			SetupSlaveSerialPort();
			Slave = ModbusSerialSlave.CreateRtu(SlaveAddress, SlaveSerialPort);
			StartSlave();

			SetupMasterSerialPort(ModbusMasterFixture.DefaultMasterSerialPortName);
			Master = ModbusSerialMaster.CreateRtu(MasterSerialPort);
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


		[Test, Ignore("Need to fix RTU slave for this function code")]
		public override void ReadWriteMultipleRegisters()
		{
			base.ReadWriteMultipleRegisters();
		}

		[Test]
		public override void ReturnQueryData()
		{
			base.ReturnQueryData();
		}
	}
}
