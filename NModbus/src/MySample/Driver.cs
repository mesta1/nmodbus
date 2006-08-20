using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using Modbus.Device;
using Modbus.Util;

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
				ReadRegisters();
				WriteRegisters();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			Console.ReadKey();
		}

		public static void ReadRegisters()
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
				ModbusASCIIMaster master = new ModbusASCIIMaster(port);

				// read five register values
				ushort startAddress = 100;
				ushort numRegisters = 5;
				ushort[] registers = master.ReadHoldingRegisters(1, startAddress, numRegisters);

				for (int	i = 0; i < numRegisters; i++)
					Console.WriteLine("Register {0}={1}", startAddress + i, registers[i]);
			}
		}

		public static void WriteRegisters()
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
				ModbusASCIIMaster master = new ModbusASCIIMaster(port);

				// write five registers			
				ushort[] registers = new ushort[] { 1, 2, 3 };
				master.WriteMultipleRegisters(1, 100, registers);
			}
		}
	}
}
