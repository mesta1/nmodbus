using System;
using System.IO.Ports;
using Modbus.Device;
using Modbus.IO;
using MbUnit.Framework;
using Rhino.Mocks;

namespace Modbus.UnitTests.Device
{
	[TestFixture]
	public class ModbusSerialMasterFixture
	{
		[Test]
		public void InitializeSerialPortTimeouts()
		{
			MockRepository mocks = new MockRepository();
			ISerialResource mockSerialResource = mocks.StrictMock<ISerialResource>();

			Expect.Call(mockSerialResource.WriteTimeout).Return(1);
			Expect.Call(mockSerialResource.InfiniteTimeout).Return(0);
			Expect.Call(mockSerialResource.WriteTimeout).Return(1);
			mockSerialResource.WriteTimeout = 1;
			Expect.Call(mockSerialResource.ReadTimeout).Return(1);
			Expect.Call(mockSerialResource.InfiniteTimeout).Return(0);
			Expect.Call(mockSerialResource.ReadTimeout).Return(1);
			mockSerialResource.ReadTimeout = 1;

			mocks.ReplayAll();
			ModbusSerialMaster.InitializeTimeouts(mockSerialResource);
			mocks.VerifyAll();
		}

		[Test]
		public void SetupTimeoutsDefaultTimeout()
		{
			MockRepository mocks = new MockRepository();
			ISerialResource mockSerialResource = mocks.StrictMock<ISerialResource>();

			Expect.Call(mockSerialResource.WriteTimeout).Return(SerialPort.InfiniteTimeout);
			Expect.Call(mockSerialResource.InfiniteTimeout).Return(SerialPort.InfiniteTimeout);
			mockSerialResource.WriteTimeout = Modbus.DefaultTimeout;
			Expect.Call(mockSerialResource.ReadTimeout).Return(SerialPort.InfiniteTimeout);
			Expect.Call(mockSerialResource.InfiniteTimeout).Return(SerialPort.InfiniteTimeout);
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
		public void CreateRtu_UsbPortFactoryMethod()
		{
			MockRepository mocks = new MockRepository();
			ISerialResource mockSerialResource = mocks.StrictMock<ISerialResource>();

			Expect.Call(mockSerialResource.WriteTimeout).Return(0);
			Expect.Call(mockSerialResource.InfiniteTimeout).Return(0);
			mockSerialResource.WriteTimeout = Modbus.DefaultTimeout;
			Expect.Call(mockSerialResource.ReadTimeout).Return(0);
			Expect.Call(mockSerialResource.InfiniteTimeout).Return(0);
			mockSerialResource.ReadTimeout = Modbus.DefaultTimeout;

			mocks.ReplayAll();
			ModbusSerialMaster.CreateRtu(mockSerialResource);
			mocks.VerifyAll();
		}

		[Test]
		public void CreateAscii_SerialPortFactoryMethod()
		{
			IModbusSerialMaster master = ModbusSerialMaster.CreateAscii(new SerialPort());

			Assert.AreEqual(Modbus.DefaultTimeout, master.Transport._serialResource.ReadTimeout);
			Assert.AreEqual(Modbus.DefaultTimeout, master.Transport._serialResource.ReadTimeout);
		}

		[Test]
		public void CreateAscii_UsbPortFactoryMethod()
		{
			MockRepository mocks = new MockRepository();
			ISerialResource mockSerialResource = mocks.StrictMock<ISerialResource>();

			Expect.Call(mockSerialResource.WriteTimeout).Return(0);
			Expect.Call(mockSerialResource.InfiniteTimeout).Return(0);
			mockSerialResource.WriteTimeout = Modbus.DefaultTimeout;
			Expect.Call(mockSerialResource.ReadTimeout).Return(0);
			Expect.Call(mockSerialResource.InfiniteTimeout).Return(0);
			mockSerialResource.ReadTimeout = Modbus.DefaultTimeout;
			mockSerialResource.NewLine = Environment.NewLine;

			mocks.ReplayAll();
			ModbusSerialMaster.CreateAscii(mockSerialResource);
			mocks.VerifyAll();
		}
	}
}
