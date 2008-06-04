using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Modbus.Device;
using MbUnit.Framework;

namespace Modbus.IntegrationTests
{
	[TestFixture]
	public class NModbusTcpSlaveFixture
	{
		[TearDown]
		public void TearDown()
		{
			// a little time for resources to become free
			Thread.Sleep(1000);
		}

		/// <summary>
		/// Tests the scenario when a slave is closed unexpectedly, causing a ConnectionResetByPeer SocketException
		/// We want to handle this gracefully - remove the master from the dictionary
		/// </summary>
		[Test]
		public void ModbusTcpSlave_ConnectionResetByPeer()
		{
			TcpListener slaveListener = new TcpListener(ModbusMasterFixture.TcpHost, ModbusMasterFixture.Port);
			slaveListener.Start();
			ModbusTcpSlave slave = ModbusTcpSlave.CreateTcp(ModbusMasterFixture.SlaveAddress, slaveListener);
			Thread slaveThread = new Thread(slave.Listen);
			slaveThread.IsBackground = true;
			slaveThread.Start();

			Thread.Sleep(500);

			using (Process masterProcess = Process.Start(Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\tools\nmodbus\TestDriver.exe")))
			{
				Thread.Sleep(2000);
				masterProcess.Kill();
			}

			Thread.Sleep(2000);
			Assert.AreEqual(0, slave.Masters.Count);
			slaveListener.Stop();
		}

		/// <summary>
		/// Tests possible exception when master closes gracefully immediately after transaction
		/// The goal is the test the exception in WriteCompleted when the slave attempts to read another request from an already closed master
		/// </summary>
		[Test]
		public void ModbusTcpSlave_ConnectionClosesGracefully()
		{
			TcpListener slaveListener = new TcpListener(ModbusMasterFixture.TcpHost, ModbusMasterFixture.Port);
			slaveListener.Start();
			ModbusTcpSlave slave = ModbusTcpSlave.CreateTcp(ModbusMasterFixture.SlaveAddress, slaveListener);
			Thread slaveThread = new Thread(slave.Listen);
			slaveThread.IsBackground = true;
			slaveThread.Start();

			using (TcpClient masterClient = new TcpClient(ModbusMasterFixture.TcpHost.ToString(), ModbusMasterFixture.Port))
			{
				ModbusIpMaster master = ModbusIpMaster.CreateTcp(masterClient);
				master.Transport.Retries = 0;

				bool[] coils = master.ReadCoils(1, 1);
				Assert.AreEqual(1, coils.Length);

				Assert.AreEqual(1, slave.Masters.Count);
			}

			// give the slave some time to remove the master
			Thread.Sleep(50);

			Assert.AreEqual(0, slave.Masters.Count);

			slaveListener.Stop();
		}

		/// <summary>
		/// Tests possible exception when master closes gracefully and the ReadHeaderCompleted EndRead call returns 0 bytes;
		/// </summary>
		[Test]
		public void ModbusTcpSlave_ConnectionSlowlyClosesGracefully()
		{
			TcpListener slaveListener = new TcpListener(ModbusMasterFixture.TcpHost, ModbusMasterFixture.Port);
			slaveListener.Start();
			ModbusTcpSlave slave = ModbusTcpSlave.CreateTcp(ModbusMasterFixture.SlaveAddress, slaveListener);
			Thread slaveThread = new Thread(slave.Listen);
			slaveThread.IsBackground = true;
			slaveThread.Start();

			using (TcpClient masterClient = new TcpClient(ModbusMasterFixture.TcpHost.ToString(), ModbusMasterFixture.Port))
			{
				ModbusIpMaster master = ModbusIpMaster.CreateTcp(masterClient);
				master.Transport.Retries = 0;

				bool[] coils = master.ReadCoils(1, 1);
				Assert.AreEqual(1, coils.Length);

				Assert.AreEqual(1, slave.Masters.Count);
				// wait a bit to let slave move on to read header
				Thread.Sleep(50);
			}

			// give the slave some time to remove the master
			Thread.Sleep(50);
			Assert.AreEqual(0, slave.Masters.Count);
			slaveListener.Stop();
		}
	}
}
