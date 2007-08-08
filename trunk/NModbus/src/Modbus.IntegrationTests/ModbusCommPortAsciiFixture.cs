using Modbus.Device;
using NUnit.Framework;

namespace Modbus.IntegrationTests
{
	[TestFixture]
	public class ModbusCommPortAsciiFixture : ModbusMasterFixture
	{
		[TestFixtureSetUp]
		public override void Init()
		{
			base.Init();

			SetupMasterSerialPort(ModbusMasterFixture.DefaultMasterSerialPortName);			
			Master = ModbusSerialMaster.CreateAscii(MasterSerialPort);
			SetupSlaveSerialPort();
			Slave = ModbusSerialSlave.CreateAscii(SlaveAddress, SlaveSerialPort);

			StartSlave();
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

		[Test]
		public void ReturnQueryData()
		{
			Assert.IsTrue(((ModbusSerialMaster) Master).ReturnQueryData(SlaveAddress, 18));
			Assert.IsTrue(((ModbusSerialMaster) Master).ReturnQueryData(SlaveAddress, 5));
		}
	}
}
