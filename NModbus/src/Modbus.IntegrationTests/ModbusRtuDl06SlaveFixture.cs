using Modbus.Device;
using NUnit.Framework;

namespace Modbus.IntegrationTests
{
	[TestFixture]
	public class ModbusRtuDl06SlaveFixture : ModbusSerialMasterFixture
	{
		[TestFixtureSetUp]
		public override void Init()
		{
			base.Init();

			SetupMasterSerialPort("COM4");
			Master = ModbusSerialMaster.CreateRtu(MasterSerialPort);
		}

		/// <summary>
		/// Not supported by the DL06
		/// </summary>
		public override void ReadWriteMultipleRegisters()
		{
		}

		/// <summary>
		/// Not supported by the DL06
		/// </summary>
		public override void ReturnQueryData()
		{
		}

		[Test, Ignore("Known Failure, TODO: enforce contstraint of zero read")]
		public override void Read0Coils()
		{
			base.Read0Coils();
		}

		[Test]
		public override void ReadCoils()
		{
			base.ReadCoils();
		}
	}
}
