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
			ISerialResource mockSerialResource = mocks.CreateMock<ISerialResource>();

			Expect.Call(mockSerialResource.WriteTimeout).Return(nonDefaultWriteTimeout);
			Expect.Call(mockSerialResource.WriteTimeout).Return(nonDefaultWriteTimeout);
			mockSerialResource.WriteTimeout = nonDefaultWriteTimeout;
			Expect.Call(mockSerialResource.ReadTimeout).Return(nonDefaultReadTimeout);
			Expect.Call(mockSerialResource.ReadTimeout).Return(nonDefaultReadTimeout);
			mockSerialResource.ReadTimeout = nonDefaultReadTimeout;

			mocks.ReplayAll();
			ModbusSerialMaster.InitializeTimeouts(mockSerialResource);
			mocks.VerifyAll();
		}

		[Test]
		public void SetupTimeoutsDefaultTimeout()
		{
			MockRepository mocks = new MockRepository();
			ISerialResource mockSerialResource = mocks.CreateMock<ISerialResource>();

			Expect.Call(mockSerialResource.WriteTimeout).Return(SerialPort.InfiniteTimeout);
			mockSerialResource.WriteTimeout = Modbus.DefaultTimeout;
			Expect.Call(mockSerialResource.ReadTimeout).Return(SerialPort.InfiniteTimeout);
			mockSerialResource.ReadTimeout = Modbus.DefaultTimeout;

			mocks.ReplayAll();
			ModbusSerialMaster.InitializeTimeouts(mockSerialResource);
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
