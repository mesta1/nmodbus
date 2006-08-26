using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using Modbus.Device;
using Modbus.Util;
using System.Net.Sockets;

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
				ModbusAsciiMasterReadRegisters();
				ModbusRtuMasterWriteRegisters();
				//ModbusTcpMasterReadRegisters();

			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			Console.ReadKey();
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

		public static void ModbusTcpMasterReadRegisters()
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
