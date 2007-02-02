using System;
using System.Collections.Generic;
using System.Text;

namespace Modbus.Device
{
	public interface IModbusTcpMaster : IModbusMaster
	{
		bool[] ReadCoils(ushort modbusAddress, ushort numberOfPoints);
		bool[] ReadInputs(ushort modbusAddress, ushort numberOfPoints);
		ushort[] ReadHoldingRegisters(ushort modbusAddress, ushort numberOfPoints);
		ushort[] ReadInputRegisters(ushort modbusAddress, ushort numberOfPoints);
		void WriteSingleCoil(ushort modbusAddress, bool value);
		void WriteSingleRegister(ushort modbusAddress, ushort value);
		void WriteMultipleRegisters(ushort modbusAddress, ushort[] data);
		void WriteMultipleCoils(ushort modbusAddress, bool[] data);
	}
}
