using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Net.Sockets;
using System.Text;
using log4net;
using Modbus.Device;
using NUnit.Framework;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace Modbus.IntegrationTests
{
	public abstract class ModbusMasterFixture
	{
		public Process Jamod;
		public IModbusMaster Master;
		public SerialPort MasterSerialPort;
		public const string MasterSerialPortName = "COM3";

		public ModbusSlave Slave;
		public Thread SlaveThread;
		public SerialPort SlaveSerialPort;
		public const string SlaveSerialPortName = "COM4";
		public const byte SlaveAddress = 1;

		public IPAddress TcpHost = new IPAddress(new byte[] { 127, 0, 0, 1 });
		public const int TcpPort = 502;
		public TcpClient MasterTcp;
		public TcpListener SlaveTcp;

		protected static readonly ILog log = LogManager.GetLogger(typeof(ModbusMasterFixture));

		public virtual void Init()
		{
			log4net.Config.XmlConfigurator.Configure();
		}

		public void SetupSlaveSerialPort()
		{
			log.DebugFormat("Configure and open slave serial port {0}.", SlaveSerialPortName);
			SlaveSerialPort = new SerialPort(SlaveSerialPortName);
			SlaveSerialPort.Parity = Parity.None;
			SlaveSerialPort.Open();
		}

		public void SetupMasterSerialPort()
		{
			log.DebugFormat("Configure and open master serial port {0}.", MasterSerialPortName);
			MasterSerialPort = new SerialPort(MasterSerialPortName);
			MasterSerialPort.Parity = Parity.None;
			MasterSerialPort.Open();
		}

		public void StartSlave()
		{
			log.Debug("Start NModbus slave.");
			SlaveThread = new Thread(Slave.Listen);
			SlaveThread.IsBackground = true;
			SlaveThread.Start();
		}

		public void StartJamodSlave(string program)
		{
			string pathToJamod = Path.Combine(Environment.CurrentDirectory, "../../../../tools/jamod");
			string classpath = String.Format(@"-classpath ""{0};{1};{2}""", Path.Combine(pathToJamod, "jamod.jar"), Path.Combine(pathToJamod, "comm.jar"), Path.Combine(pathToJamod, "."));
			ProcessStartInfo startInfo = new ProcessStartInfo("java", String.Format("{0} {1}", classpath, program));
			Jamod = Process.Start(startInfo);

			Thread.Sleep(2000);
			Assert.IsFalse(Jamod.HasExited, "Jamod Serial Ascii Slave did not start correctly.");
			
			// TODO test whether the slave has opened the serial port
		}

		[TestFixtureTearDown]
		public void CleanUp()
		{
			log.Debug("Clean up after tests.");

			if (MasterSerialPort != null && MasterSerialPort.IsOpen)
				MasterSerialPort.Dispose();

			if (SlaveSerialPort != null && SlaveSerialPort.IsOpen)
				SlaveSerialPort.Dispose();			

			if (Jamod != null)
			{
				Jamod.CloseMainWindow();
				Jamod.Close();
			}

			Thread.Sleep(1000);
		}

		[Test]
		public virtual void ReadCoils()
		{
			bool[] coils = Master.ReadCoils(SlaveAddress, 1, 8);
			Assert.AreEqual(new bool[] { false, false, false, false, false, false, false, false }, coils);
		}

		[Test]
		public virtual void Read0Coils()
		{
			bool[] coils = Master.ReadCoils(SlaveAddress, 100, 0);
			Assert.AreEqual(new bool[] { }, coils);
		}

		[Test]
		public virtual void ReadInputs()
		{
			bool[] inputs = Master.ReadInputs(SlaveAddress, 150, 3);
			Assert.AreEqual(new bool[] { false, false, false }, inputs);
		}

		[Test]
		public virtual void ReadHoldingRegisters()
		{
			ushort[] registers = Master.ReadHoldingRegisters(SlaveAddress, 104, 2);
			Assert.AreEqual(new ushort[] { 0, 0 }, registers);
		}

		[Test]
		public virtual void ReadInputRegisters()
		{
			ushort[] registers = Master.ReadInputRegisters(SlaveAddress, 104, 2);
			Assert.AreEqual(new ushort[] { 0, 0 }, registers);
		}

		[Test]
		public virtual void WriteSingleCoil()
		{
			bool coilValue = Master.ReadCoils(SlaveAddress, 10, 1)[0];
			Master.WriteSingleCoil(SlaveAddress, 10, !coilValue);
			Assert.AreEqual(!coilValue, Master.ReadCoils(SlaveAddress, 10, 1)[0]);
			Master.WriteSingleCoil(SlaveAddress, 10, coilValue);
			Assert.AreEqual(coilValue, Master.ReadCoils(SlaveAddress, 10, 1)[0]);
		}

		[Test]
		public virtual void WriteSingleRegister()
		{
			ushort testAddress = 200;
			ushort testValue = 350;

			ushort originalValue = Master.ReadHoldingRegisters(SlaveAddress, testAddress, 1)[0];
			Master.WriteSingleRegister(SlaveAddress, testAddress, testValue);
			Assert.AreEqual(testValue, Master.ReadHoldingRegisters(SlaveAddress, testAddress, 1)[0]);
			Master.WriteSingleRegister(SlaveAddress, testAddress, originalValue);
			Assert.AreEqual(originalValue, Master.ReadHoldingRegisters(SlaveAddress, testAddress, 1)[0]);
		}

		[Test]
		public virtual void WriteMultipleRegisters()
		{
			ushort testAddress = 120;
			ushort[] testValues = new ushort[] { 10, 20, 30, 40, 50 };

			ushort[] originalValues = Master.ReadHoldingRegisters(SlaveAddress, testAddress, (ushort) testValues.Length);
			Master.WriteMultipleRegisters(SlaveAddress, testAddress, testValues);
			ushort[] newValues = Master.ReadHoldingRegisters(SlaveAddress, testAddress, (ushort) testValues.Length);
			Assert.AreEqual(testValues, newValues);
			Master.WriteMultipleRegisters(SlaveAddress, testAddress, originalValues);
		}

		[Test]
		public virtual void WriteMultipleCoils()
		{
			ushort testAddress = 200;
			bool[] testValues = new bool[] { true, false, true, false, false, false, true, false, true, false };

			bool[] originalValues = Master.ReadCoils(SlaveAddress, testAddress, (ushort) testValues.Length);
			Master.WriteMultipleCoils(SlaveAddress, testAddress, testValues);
			bool[] newValues = Master.ReadCoils(SlaveAddress, testAddress, (ushort) testValues.Length);
			Assert.AreEqual(testValues, newValues);
			Master.WriteMultipleCoils(SlaveAddress, testAddress, originalValues);
		}

		// TODO how can we test a slave exception?
	}
}
