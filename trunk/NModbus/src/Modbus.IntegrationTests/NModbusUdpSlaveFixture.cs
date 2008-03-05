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
	public class NModbusUdpSlaveFixture
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(NModbusUdpSlaveFixture));

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

			UdpClient slaveClient = CreateAndStartUdpSlave(ModbusMasterFixture.Port, DataStoreFactory.CreateTestDataStore());
			
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

		[Test, Ignore("TODO consider supporting this scenario")]
		public void ModbusUdpSlave_SingleMasterPollingMultipleSlaves()
		{			
			DataStore slave1DataStore = new DataStore();
			slave1DataStore.CoilDiscretes.Add(true);

			DataStore slave2DataStore = new DataStore();
			slave2DataStore.CoilDiscretes.Add(false);

			using (UdpClient slave1 = CreateAndStartUdpSlave(502, slave1DataStore))
			using (UdpClient slave2 = CreateAndStartUdpSlave(503, slave2DataStore))
			using (UdpClient masterClient = new UdpClient())
			{			
				masterClient.Connect(ModbusMasterFixture.DefaultModbusIPEndPoint);
				ModbusIpMaster master = ModbusIpMaster.CreateUdp(masterClient);

				for (int i = 0; i < 5; i++)
				{
					// we would need to create an overload taking in a port argument
					Assert.IsTrue(master.ReadCoils(0, 1)[0]);
					Assert.IsFalse(master.ReadCoils(1, 1)[0]);
				}
			}
		}

		private UdpClient CreateAndStartUdpSlave(int port, DataStore dataStore)
		{
			UdpClient slaveClient = new UdpClient(port);
			ModbusSlave slave = ModbusUdpSlave.CreateUdp(slaveClient);
			slave.DataStore = dataStore;
			Thread slaveThread = new Thread(slave.Listen);
			slaveThread.Start();

			return slaveClient;
		}
	}
}
