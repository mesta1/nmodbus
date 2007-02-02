using System;
using System.Collections.Generic;
using System.Text;
using Modbus.IO;

namespace Modbus.Device
{
	public interface IModbusMaster
	{
		ModbusTransport Transport { get; }
		bool[] ReadCoils(byte slaveAddress, ushort modbusAddress, ushort numberOfPoints);
		bool[] ReadInputs(byte slaveAddress, ushort modbusAddress, ushort numberOfPoints);
		ushort[] ReadHoldingRegisters(byte slaveAddress, ushort modbusAddress, ushort numberOfPoints);
		ushort[] ReadInputRegisters(byte slaveAddress, ushort modbusAddress, ushort numberOfPoints);
		void WriteSingleCoil(byte slaveAddress, ushort modbusAddress, bool value);
		void WriteSingleRegister(byte slaveAddress, ushort modbusAddress, ushort value);
		void WriteMultipleRegisters(byte slaveAddress, ushort modbusAddress, ushort[] data);
		void WriteMultipleCoils(byte slaveAddress, ushort modbusAddress, bool[] data);		
	}
}
