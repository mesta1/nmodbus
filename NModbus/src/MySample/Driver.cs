using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Net.Sockets;
using System.Threading;
using Modbus.Device;
using Modbus.Util;
using Modbus.IO;

namespace MySample
{
	/// <summary>
	/// Demonstration of NModbus.
	/// </summary>
	public class Driver
	{
		static void Main(string[] args)
		{
			try
			{
				ModbusSerialRtuMasterReadRegisters();
				//ModbusAsciiMasterReadRegistersFromModbusSlave();				
				//StartModbusAsciiSlave();
				//ModbusAsciiMasterReadRegisters();
			
				//ModbusAsciiMasterReadRegistersFromModbusSlave();
				//ModbusTcpMasterSample();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			Console.ReadKey();
		}

		/// <summary>
		/// Modbus serial ASCII master and slave example.
		/// </summary>
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
				ModbusSlave slave = ModbusSerialSlave.CreateAscii(slaveID, slavePort);
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
				ModbusSlave slave = ModbusSerialSlave.CreateAscii(slaveID, slavePort);
				slave.Listen();
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

				// write three registers			
				ushort[] registers = new ushort[] { 1, 2, 3 };
				master.WriteMultipleRegisters(1, 100, registers);
			}
		}

		/// <summary>
		/// Simple Modbus serial ASCII master read holding registers example.
		/// </summary>
		public static void ModbusSerialRtuMasterReadRegisters()
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

				// read five registers			
				master.ReadHoldingRegisters(1, 100, 5);
			}

			// output: 
			// 
		}		

		public static void ModbusTcpMasterSample()
		{
			using (TcpClient client = new TcpClient("127.0.0.1", 502))
			{
				ModbusTcpMaster master = ModbusTcpMaster.CreateTcp(client);

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
