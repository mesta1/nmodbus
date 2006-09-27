using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using Modbus.Device;
using Modbus.Util;
using System.Net.Sockets;
using Modbus.IO;
using System.Threading;

namespace MySample
{
	/// <summary>
	/// Demonstration of NModbus.
	/// </summary>
	class Driver
	{
		static void Main(string[] args)
		{
			try
			{
				//ModbusAsciiMasterReadRegistersFromModbusSlave();
				//StartModbusAsciiSlave();
				//ModbusAsciiMasterReadRegisters();
				//ModbusRtuMasterWriteRegisters();
				ModbusTcpMasterSample();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			Console.ReadKey();
		}

		public static void StartModbusAsciiSlave()
		{
			using (SerialPort slavePort = new SerialPort("COM1"))
			{
				// configure serial port
				slavePort.BaudRate = 9600;
				slavePort.DataBits = 8;
				slavePort.Parity = Parity.None;
				slavePort.StopBits = StopBits.One;
				slavePort.Open();

				// create modbus slave
				byte slaveID = 1;
				ModbusSlave slave = ModbusSlave.CreateAscii(slaveID, slavePort);
				slave.Listen();
			}
		}

		public static void ModbusAsciiMasterReadRegistersFromModbusSlave()
		{
			Thread slaveThread;

			using (SerialPort masterPort = new SerialPort("COM1"))
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
				ModbusSlave slave = ModbusSlave.CreateAscii(slaveID, slavePort);
				slaveThread = new Thread(new ThreadStart(slave.Listen));
				slaveThread.Start();

				// create modbus master
				ModbusSerialMaster master = ModbusSerialMaster.CreateAscii(masterPort);

				master.Transport.Retries = 5;
				
				// read five register values
				ushort startAddress = 100;
				ushort numRegisters = 5;
				ushort[] registers = master.ReadHoldingRegisters(slaveID, startAddress, numRegisters);

				for (int i = 0; i < numRegisters; i++)
					Console.WriteLine("Register {0}={1}", startAddress + i, registers[i]);
			}

			slaveThread.Abort();
		}

		public static void ModbusAsciiMasterReadRegisters()
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

				// read five register values
				ushort startAddress = 100;
				ushort numRegisters = 5;
				ushort[] registers = master.ReadHoldingRegisters(1, startAddress, numRegisters);

				for (int i = 0; i < numRegisters; i++)
					Console.WriteLine("Register {0}={1}", startAddress + i, registers[i]);
			}
		}

		public static void ModbusRtuMasterWriteRegisters()
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

				// write five registers			
				ushort[] registers = new ushort[] { 1, 2, 3 };
				master.WriteMultipleRegisters(1, 100, registers);
			}
		}

		public static void ModbusTcpMasterSample()
		{
			using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
			{
				socket.Connect("127.0.0.1", 502);

				ModbusTcpMaster master = ModbusTcpMaster.CreateTcp(socket);

				// read five register values
				ushort startAddress = 100;
				ushort numRegisters = 5;
				ushort[] registers = master.ReadHoldingRegisters(startAddress, numRegisters);

				for (int i = 0; i < numRegisters; i++)
					Console.WriteLine("Register {0}={1}", startAddress + i, registers[i]);
			}
		}
	}
}
