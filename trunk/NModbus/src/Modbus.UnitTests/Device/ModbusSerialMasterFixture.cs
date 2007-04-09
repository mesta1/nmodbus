using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using Modbus.IO;
using Modbus.Device;
using System.IO.Ports;

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
			SerialPortAdapter mockSerialPort = mocks.CreateMock<SerialPortAdapter>();

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
			SerialPortAdapter mockSerialPort = mocks.CreateMock<SerialPortAdapter>();

			Expect.Call(mockSerialPort.WriteTimeout).Return(SerialPort.InfiniteTimeout);
			mockSerialPort.WriteTimeout = Modbus.DefaultTimeout;
			Expect.Call(mockSerialPort.ReadTimeout).Return(SerialPort.InfiniteTimeout);
			mockSerialPort.ReadTimeout = Modbus.DefaultTimeout;

			mocks.ReplayAll();
			ModbusSerialMaster.InitializeSerialPortTimeouts(mockSerialPort);
			mocks.VerifyAll();
		}
	}
}
