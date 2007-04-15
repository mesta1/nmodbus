using Modbus.Device;
using NUnit.Framework;

namespace Modbus.IntegrationTests
{
	[TestFixture]
	public class ModbusRtuDl06SlaveFixture : ModbusMasterFixture
	{
		[TestFixtureSetUp]
		public override void Init()
		{
			base.Init();

			SetupMasterSerialPort("COM4");
			Master = ModbusSerialMaster.CreateRtu(MasterSerialPort);
		}

		[Test, Ignore("Known Failure, TODO: enforce contstraint of zero read")]
		public override void Read0Coils()
		{
			base.Read0Coils();
		}
	}
}
