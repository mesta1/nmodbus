using NUnit.Framework;
using System.Net.Sockets;
using Modbus.Device;
using System;
using System.Threading;
using Modbus.Data;
using log4net;

namespace Modbus.IntegrationTests
{
	[TestFixture]
	public class ModbusUdpSlaveFixture
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(ModbusUdpSlaveFixture));

		[TestFixtureSetUp]
		public void TestFixtureSetup()
		{
			log4net.Config.XmlConfigurator.Configure();
		}

		[TearDown]
		public void TearDown()
		{
			// a little time for resources to become free
			Thread.Sleep(1000);
		}

		[Test]
		public void ModbusUdpSlave_StartAndStopSlaveBeforeAnyRequests()
		{
			using (UdpClient client = new UdpClient(ModbusMasterFixture.Port))
			{
				ModbusSlave slave = ModbusUdpSlave.CreateUdp(1, client);
				slave.Listen();
			}
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void ModbusUdpSlave_NotBound()
		{
			UdpClient client = new UdpClient();
			ModbusSlave slave = ModbusUdpSlave.CreateUdp(1, client);
			slave.Listen();
		}

		[Test]
		public void ModbusUdpSlave_MultipleMasters()
		{
			Random randomNumberGenerator = new Random();
			bool master1Complete = false;
			bool master2Complete = false;
			UdpClient masterClient1 = new UdpClient();
			masterClient1.Connect(ModbusMasterFixture.DefaultModbusIPEndPoint);
			ModbusIpMaster master1 = ModbusIpMaster.CreateUdp(masterClient1);

			UdpClient masterClient2 = new UdpClient();
			masterClient2.Connect(ModbusMasterFixture.DefaultModbusIPEndPoint);
			ModbusIpMaster master2 = ModbusIpMaster.CreateUdp(masterClient2);

			UdpClient slaveClient = new UdpClient(ModbusMasterFixture.Port);
			ModbusSlave slave = ModbusUdpSlave.CreateUdp(slaveClient);
			slave.DataStore = DataStoreFactory.CreateTestDataStore();
			Thread slaveThread = new Thread(slave.Listen);
			slaveThread.Start();

			Thread master1Thread = new Thread(delegate() 
			{
				for (int i = 0; i < 5; i++)
				{
					Thread.Sleep(randomNumberGenerator.Next(1000));
					_log.Debug("Read from master 1");
					Assert.AreEqual(new ushort[] { 2, 3, 4, 5, 6 }, master1.ReadHoldingRegisters(1, 5));
				}
				master1Complete = true;
			});

			Thread master2Thread = new Thread(delegate()
			{
				for (int i = 0; i < 5; i++)
				{
					Thread.Sleep(randomNumberGenerator.Next(1000));
					_log.Debug("Read from master 2");
					Assert.AreEqual(new ushort[] { 3, 4, 5, 6, 7 }, master2.ReadHoldingRegisters(2, 5));
				}
				master2Complete = true;
			});

			master1Thread.Start();
			master2Thread.Start();

			while (!master1Complete || !master2Complete)
			{
				Thread.Sleep(200);
			}

			slaveClient.Close();
			masterClient1.Close();
			masterClient2.Close();
		}
	}
}
