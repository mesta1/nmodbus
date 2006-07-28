using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Message;

namespace Modbus.UnitTests.Message
{
	[TestFixture]
	public class ModbusMessageImplFixture
	{
		[Test]
		public void CheckModbusMessageCtorInitializesProperties()
		{
			ModbusMessageImpl messageImpl = new ModbusMessageImpl(5, Modbus.READ_COILS);
			Assert.AreEqual(5, messageImpl.SlaveAddress);
			Assert.AreEqual(Modbus.READ_COILS, messageImpl.FunctionCode);
		}

		[Test]
		public void CheckInitialize()
		{
			ModbusMessageImpl messageImpl = new ModbusMessageImpl();
			messageImpl.Initialize(new byte[] { 1, 2, 9, 9, 9, 9 });
			Assert.AreEqual(1, messageImpl.SlaveAddress);
			Assert.AreEqual(2, messageImpl.FunctionCode);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ChecckInitializeFrameNull()
		{
			ModbusMessageImpl messageImpl = new ModbusMessageImpl();
			messageImpl.Initialize(null);
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void CheckInitializeInvalidFrame()
		{
			ModbusMessageImpl messageImpl = new ModbusMessageImpl();
			messageImpl.Initialize(new byte[] { 1 });
		}

		[Test]
		public void CheckProtocolDataUnit()
		{
			ModbusMessageImpl messageImpl = new ModbusMessageImpl(11, Modbus.READ_COILS);
			byte[] expectedResult = new byte[] { Modbus.READ_COILS };
			Assert.AreEqual(expectedResult, messageImpl.ProtocolDataUnit);
		}

		[Test]
		public void CheckChecksumBody()
		{
			ModbusMessageImpl messageImpl = new ModbusMessageImpl(11, Modbus.READ_HOLDING_REGISTERS);
			byte[] expectedChecksumBody = new byte[] { 11, Modbus.READ_HOLDING_REGISTERS };
			Assert.AreEqual(expectedChecksumBody, messageImpl.ChecksumBody);
		}		
	}
}
