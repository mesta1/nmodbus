using Modbus.Message;
using NUnit.Framework;

namespace Modbus.UnitTests.Message
{
	[TestFixture]
	public class ModbusMessageFixture
	{
		[Test]
		public void ProtocolDataUnitReadCoilsRequest()
		{
			ModbusMessage message = new ReadCoilsInputsRequest(Modbus.ReadCoils, 1, 100, 9);
			byte[] expectedResult = { Modbus.ReadCoils, 0, 100, 0, 9 };
			Assert.AreEqual(expectedResult, message.ProtocolDataUnit);
		}

		[Test]
		public void MessageFrameReadCoilsRequest()
		{
			ModbusMessage message = new ReadCoilsInputsRequest(Modbus.ReadCoils, 1, 2, 3);
			byte[] expectedMessageFrame = { 1, Modbus.ReadCoils, 0, 2, 0, 3 };
			Assert.AreEqual(expectedMessageFrame, message.MessageFrame);
		}

		internal void AssertModbusMessagePropertiesAreEqual(IModbusMessage obj1, IModbusMessage obj2)
		{
			Assert.AreEqual(obj1.FunctionCode, obj2.FunctionCode);
			Assert.AreEqual(obj1.SlaveAddress, obj2.SlaveAddress);
			Assert.AreEqual(obj1.MessageFrame, obj2.MessageFrame);
			Assert.AreEqual(obj1.ProtocolDataUnit, obj2.ProtocolDataUnit);
		}
	}
}
