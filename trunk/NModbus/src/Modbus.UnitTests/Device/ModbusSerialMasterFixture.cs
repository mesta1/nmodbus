using System.IO.Ports;
using Modbus.Device;
using Modbus.IO;
using NUnit.Framework;
using Rhino.Mocks;

namespace Modbus.UnitTests.Device
{
	[TestFixture]
	public class ModbusSerialMasterFixture
	{
		[Test]
		public void InitializeSerialPortTimeouts()
		{
			int nonDefaultReadTimeout = 67;
			int nonDefaultWriteTimeout = 42;
			MockRepository mocks = new MockRepository();
			CommPortAdapter mockSerialPort = mocks.CreateMock<CommPortAdapter>();

			Expect.Call(mockSerialPort.WriteTimeout).Return(nonDefaultWriteTimeout);
			Expect.Call(mockSerialPort.WriteTimeout).Return(nonDefaultWriteTimeout);
			mockSerialPort.WriteTimeout = nonDefaultWriteTimeout;
			Expect.Call(mockSerialPort.ReadTimeout).Return(nonDefaultReadTimeout);
			Expect.Call(mockSerialPort.ReadTimeout).Return(nonDefaultReadTimeout);
			mockSerialPort.ReadTimeout = nonDefaultReadTimeout;

			mocks.ReplayAll();
			ModbusSerialMaster.InitializeSerialPortTimeouts(mockSerialPort);
			mocks.VerifyAll();
		}

		[Test]
		public void SetupTimeoutsDefaultTimeout()
		{
			MockRepository mocks = new MockRepository();
			CommPortAdapter mockSerialPort = mocks.CreateMock<CommPortAdapter>();

			Expect.Call(mockSerialPort.WriteTimeout).Return(SerialPort.InfiniteTimeout);
			mockSerialPort.WriteTimeout = Modbus.DefaultTimeout;
			Expect.Call(mockSerialPort.ReadTimeout).Return(SerialPort.InfiniteTimeout);
			mockSerialPort.ReadTimeout = Modbus.DefaultTimeout;

			mocks.ReplayAll();
			ModbusSerialMaster.InitializeSerialPortTimeouts(mockSerialPort);
			mocks.VerifyAll();
		}

		[Test]
		public void CreateRtu_SerialPortFactoryMethod()
		{
			IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(new SerialPort());

			Assert.AreEqual(Modbus.DefaultTimeout, master.Transport._serialResource.ReadTimeout);
		}

		[Test]
		public void CreateAscii_SerialPortFactoryMethod()
		{
			IModbusSerialMaster master = ModbusSerialMaster.CreateAscii(new SerialPort());

			Assert.AreEqual(Modbus.DefaultTimeout, master.Transport._serialResource.ReadTimeout);
		}
	}
}
