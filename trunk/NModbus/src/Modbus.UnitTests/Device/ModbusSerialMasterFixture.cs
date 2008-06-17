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
		public void CreateRtu_SerialPortFactoryMethod()
		{
			IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(new SerialPort());

			Assert.AreEqual(Modbus.DefaultTimeout, master.Transport.StreamResource.ReadTimeout);
		}

		[Test]
		public void CreateRtu_UsbPortFactoryMethod()
		{
			MockRepository mocks = new MockRepository();
			IStreamResource mockStreamResource = mocks.StrictMock<IStreamResource>();

			Expect.Call(mockStreamResource.WriteTimeout).Return(0);
			Expect.Call(mockStreamResource.InfiniteTimeout).Return(0);
			mockStreamResource.WriteTimeout = Modbus.DefaultTimeout;
			Expect.Call(mockStreamResource.ReadTimeout).Return(0);
			Expect.Call(mockStreamResource.InfiniteTimeout).Return(0);
			mockStreamResource.ReadTimeout = Modbus.DefaultTimeout;

			mocks.ReplayAll();
			var master = ModbusSerialMaster.CreateRtu(mockStreamResource);
			mocks.VerifyAll();
		}

		[Test]
		public void CreateAscii_SerialPortFactoryMethod()
		{
			IModbusSerialMaster master = ModbusSerialMaster.CreateAscii(new SerialPort());

			Assert.AreEqual(Modbus.DefaultTimeout, master.Transport.StreamResource.ReadTimeout);
			Assert.AreEqual(Modbus.DefaultTimeout, master.Transport.StreamResource.ReadTimeout);
		}

		[Test]
		public void CreateAscii_UsbPortFactoryMethod()
		{
			MockRepository mocks = new MockRepository();
			IStreamResource mockSerialResource = mocks.StrictMock<IStreamResource>();

			Expect.Call(mockSerialResource.WriteTimeout).Return(0);
			Expect.Call(mockSerialResource.InfiniteTimeout).Return(0);
			mockSerialResource.WriteTimeout = Modbus.DefaultTimeout;
			Expect.Call(mockSerialResource.ReadTimeout).Return(0);
			Expect.Call(mockSerialResource.InfiniteTimeout).Return(0);
			mockSerialResource.ReadTimeout = Modbus.DefaultTimeout;

			mocks.ReplayAll();
			ModbusSerialMaster.CreateAscii(mockSerialResource);
			mocks.VerifyAll();
		}
	}
}
