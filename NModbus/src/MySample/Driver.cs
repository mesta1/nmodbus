using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Modbus.Device;
using Modbus.Util;

namespace MySample
{
	/// <summary>
	/// Demonstration of NModbus
	/// </summary>
	public class Driver
	{
		static void Main(string[] args)
		{
			//log4net.Config.XmlConfigurator.Configure();

			try
			{
				ModbusTcpMasterReadInputs();
				//SimplePerfTest();
				//ModbusSerialRtuMasterWriteRegisters();
				//ModbusSerialAsciiMasterReadRegisters();
				//ModbusTcpMasterReadInputs();				
				//StartModbusAsciiSlave();
				//ModbusTcpMasterReadInputsFromModbusSlave();
				//ModbusSerialAsciiMasterReadRegistersFromModbusSlave();
				//StartModbusTcpSlave();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			//Console.ReadKey();
		}

		public static void SimplePerfTest()
		{
			using (SerialPort port = new SerialPort("COM4"))
			{
				// configure serial port
				port.BaudRate = 9600;
				port.DataBits = 8;
				port.Parity = Parity.None;
				port.StopBits = StopBits.One;
				port.Open();

				// create modbus master
				ModbusSerialMaster master = ModbusSerialMaster.CreateRtu(port);

				byte slaveID = 1;
				ushort startAddress = 5;
				ushort numRegisters = 5;

				// JIT compile the IL
				master.ReadHoldingRegisters(slaveID, startAddress, numRegisters);

				Stopwatch stopwatch = new Stopwatch();
				long sum = 0;
				double numberOfReads = 1000;

				for (int i = 0; i < numberOfReads; i++)
				{
					stopwatch.Reset();
					stopwatch.Start();
					ushort[] registers = master.ReadHoldingRegisters(slaveID, startAddress, numRegisters);
					stopwatch.Stop();
					Console.WriteLine(String.Format("Read {0} - {1} milliseconds", i + 1, stopwatch.ElapsedMilliseconds));
					sum += stopwatch.ElapsedMilliseconds;
				}

				Console.WriteLine(String.Format("Average {0}ms", sum / numberOfReads));
			}
		}

		/// <summary>
		/// Simple Modbus serial RTU master write holding registers example.
		/// </summary>
		public static void ModbusSerialRtuMasterWriteRegisters()
		{
			using (SerialPort port = new SerialPort("COM5"))
			{
				// configure serial port
				port.BaudRate = 9600;
				port.DataBits = 8;
				port.Parity = Parity.None;
				port.StopBits = StopBits.One;
				port.Open();

				// create modbus master
				ModbusSerialMaster master = ModbusSerialMaster.CreateRtu(port);
							
				byte slaveID = 1;
				ushort startAddress = 100;
				ushort[] registers = new ushort[] { 1, 2, 3 };

				// write three registers
				master.WriteMultipleRegisters(slaveID, startAddress, registers);
			}
		}

		/// <summary>
		/// Simple Modbus serial ASCII master read holding registers example.
		/// </summary>
		public static void ModbusSerialAsciiMasterReadRegisters()
		{
			using (SerialPort port = new SerialPort("COM5"))
			{
				// configure serial port
				port.BaudRate = 9600;
				port.DataBits = 8;
				port.Parity = Parity.None;
				port.StopBits = StopBits.One;
				port.Open();

				// create modbus master
				ModbusSerialMaster master = ModbusSerialMaster.CreateAscii(port);

				byte slaveID = 1;
				ushort startAddress = 1;
				ushort numRegisters = 5;

				// read five registers		
				ushort[] registers = master.ReadHoldingRegisters(slaveID, startAddress, numRegisters);

				for (int i = 0; i < numRegisters; i++)
					Console.WriteLine("Register {0}={1}", startAddress + i, registers[i]);
			}

			// output: 
			// Register 1=0
			// Register 2=0
			// Register 3=0
			// Register 4=0
			// Register 5=0
		}
		
		/// <summary>
		/// Simple Modbus TCP master read inputs example.
		/// </summary>
		public static void ModbusTcpMasterReadInputs()
		{
			using (TcpClient client = new TcpClient("127.0.0.1", 502))
			{
				client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
				ModbusTcpMaster master = ModbusTcpMaster.CreateTcp(client);

				// read five input values
				ushort startAddress = 100;
				ushort numInputs = 5;
				bool[] inputs = master.ReadInputs(startAddress, numInputs);

				for (int i = 0; i < numInputs; i++)
					Console.WriteLine("Input {0}={1}", startAddress + i, inputs[i] ? 1 : 0);

				while (true)
				{
				}
			}

			// output: 
			// Input 100=0
			// Input 101=0
			// Input 102=0
			// Input 103=0
			// Input 104=0
		}

		/// <summary>
		/// Simple Modbus Serial ASCII slave example.
		/// </summary>
		public static void StartModbusAsciiSlave()
		{
			using (SerialPort slavePort = new SerialPort("COM4"))
			{
				// configure serial port
				slavePort.BaudRate = 9600;
				slavePort.DataBits = 8;
				slavePort.Parity = Parity.None;
				slavePort.StopBits = StopBits.One;
				slavePort.Open();
				
				byte unitID = 1;

				// create modbus slave
				ModbusSlave slave = ModbusSerialSlave.CreateAscii(unitID, slavePort);
				
				slave.Listen();
			}
		}

		/// <summary>
		/// Simple Modbus TCP slave example.
		/// </summary>
		public static void StartModbusTcpSlave()
		{
			byte slaveID = 1;
			int port = 502;
			IPAddress address = new IPAddress(new byte[] { 127, 0, 0, 1 });

			// create and start the TCP slave
			TcpListener slaveTcpListener = new TcpListener(address, port);
			slaveTcpListener.Start();
			ModbusSlave slave = ModbusTcpSlave.CreateTcp(slaveID, slaveTcpListener);			
			slave.Listen();
		}

		/// <summary>
		/// Modbus TCP master and slave example.
		/// </summary>
		public static void ModbusTcpMasterReadInputsFromModbusSlave()
		{
			byte slaveID = 1;
			int port = 502;
			IPAddress address = new IPAddress(new byte[] { 127, 0, 0, 1 });
			
			// create and start the TCP slave
			TcpListener slaveTcpListener = new TcpListener(address, port);
			slaveTcpListener.Start();
			ModbusSlave slave = ModbusTcpSlave.CreateTcp(slaveID, slaveTcpListener);
			Thread slaveThread = new Thread(slave.Listen);
			slaveThread.Start();

			// create the master
			TcpClient masterTcpClient = new TcpClient(address.ToString(), port);
			ModbusTcpMaster master = ModbusTcpMaster.CreateTcp(masterTcpClient);	

			ushort numInputs = 5;
			ushort startAddress = 100;

			// read five register values
			ushort[] inputs = master.ReadInputRegisters(startAddress, numInputs);

			for (int i = 0; i < numInputs; i++)
				Console.WriteLine("Register {0}={1}", startAddress + i, inputs[i]);
			
			// clean up
			masterTcpClient.Close();
			slaveTcpListener.Stop();

			// output
			// Register 100=0
			// Register 101=0
			// Register 102=0
			// Register 103=0
			// Register 104=0
		}

		/// <summary>
		/// Modbus serial ASCII master and slave example.
		/// </summary>
		public static void ModbusSerialAsciiMasterReadRegistersFromModbusSlave()
		{
			Thread slaveThread;

			using (SerialPort masterPort = new SerialPort("COM6"))
			using (SerialPort slavePort = new SerialPort("COM5"))
			{
				// configure serial ports
				masterPort.BaudRate = slavePort.BaudRate = 9600;
				masterPort.DataBits = slavePort.DataBits = 8;
				masterPort.Parity = slavePort.Parity = Parity.None;
				masterPort.StopBits = slavePort.StopBits = StopBits.One;
				masterPort.Open();
				slavePort.Open();

				// create modbus slave on seperate thread
				byte slaveID = 1;
				ModbusSlave slave = ModbusSerialSlave.CreateAscii(slaveID, slavePort);
				slaveThread = new Thread(new ThreadStart(slave.Listen));
				slaveThread.Start();

				// create modbus master
				ModbusSerialMaster master = ModbusSerialMaster.CreateAscii(masterPort);

				master.Transport.Retries = 5;
				ushort startAddress = 100;
				ushort numRegisters = 5;

				// read five register values
				ushort[] registers = master.ReadHoldingRegisters(slaveID, startAddress, numRegisters);

				for (int i = 0; i < numRegisters; i++)
					Console.WriteLine("Register {0}={1}", startAddress + i, registers[i]);
			}			

			// output
			// Register 100=0
			// Register 101=0
			// Register 102=0
			// Register 103=0
			// Register 104=0
		}

		public static void ReadWrite32BitValue()
		{
			using (SerialPort port = new SerialPort("COM6"))
			{
				// configure serial port
				port.BaudRate = 9600;
				port.DataBits = 8;
				port.Parity = Parity.None;
				port.StopBits = StopBits.One;
				port.Open();

				// create modbus master
				ModbusSerialMaster master = ModbusSerialMaster.CreateRtu(port);

				byte slaveID = 1;
				ushort startAddress = 1008;
				uint largeValue = UInt16.MaxValue + 5;

				ushort lowOrderValue = BitConverter.ToUInt16(BitConverter.GetBytes(largeValue), 0);
				ushort highOrderValue = BitConverter.ToUInt16(BitConverter.GetBytes(largeValue), 2);							

				// write large value in two 16 bit chunks
				master.WriteMultipleRegisters(slaveID, startAddress, new ushort[] { lowOrderValue, highOrderValue});

				// read large value in two 16 bit chunks and perform conversion
				ushort[] registers = master.ReadHoldingRegisters(slaveID, startAddress, 2);
				uint value = ModbusUtil.GetUInt32(registers[1], registers[0]);
			}
		}
	}
}
