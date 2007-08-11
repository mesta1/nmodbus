using NUnit.Framework;
using Modbus.Device;

namespace Modbus.IntegrationTests
{
	[TestFixture]
	public class ModbusSerialMasterFixture : ModbusMasterFixture
	{
		[Test]
		public virtual void ReturnQueryData()
		{
			Assert.IsTrue(((ModbusSerialMaster) Master).ReturnQueryData(SlaveAddress, 18));
			Assert.IsTrue(((ModbusSerialMaster) Master).ReturnQueryData(SlaveAddress, 5));
		}
	}
}
