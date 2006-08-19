using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using Modbus.Device;
using Modbus.Util;

namespace MySample
{
	class Driver
	{
		static void Main(string[] args)
		{
			try
			{
				ReadHoldingRegisters();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			Console.ReadKey();
		}

		public static void ReadHoldingRegisters()
		{
			using (SerialPort port = new SerialPort("COM4"))
			{
				// configure serial port
				port.BaudRate = 9600;
				port.DataBits = 8;
				port.Parity = Parity.None;
				port.StopBits = StopBits.One;
				port.Encoding = Encoding.ASCII;

				// open the port
				port.Open();

				// create modbus master
				ModbusASCIIMaster master = new ModbusASCIIMaster(port);
				
				// read five register values
				ushort startAddress = 100;
				ushort[] registers = master.ReadHoldingRegisters(1, startAddress, 5);

				Console.WriteLine(StringUtil.Join(Environment.NewLine, registers, 
					delegate(ushort registerValue) { return String.Format("Register {0}={1}", startAddress++, registerValue); }));
			}
		}
	}
}
