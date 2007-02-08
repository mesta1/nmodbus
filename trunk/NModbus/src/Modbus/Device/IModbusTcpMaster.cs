using System;
using System.Collections.Generic;
using System.Text;

namespace Modbus.Device
{
	public interface IModbusTcpMaster : IModbusMaster
	{
		bool[] ReadCoils(ushort startAddress, ushort numberOfPoints);
		bool[] ReadInputs(ushort startAddress, ushort numberOfPoints);
		ushort[] ReadHoldingRegisters(ushort startAddress, ushort numberOfPoints);
		ushort[] ReadInputRegisters(ushort startAddress, ushort numberOfPoints);
		void WriteSingleCoil(ushort coilAddress, bool value);
		void WriteSingleRegister(ushort registerAddress, ushort value);
		void WriteMultipleRegisters(ushort startAddress, ushort[] data);
		void WriteMultipleCoils(ushort startAddress, bool[] data);
		ushort[] ReadWriteMultipleRegisters(ushort startReadAddress, ushort numberOfPointsToRead, ushort startWriteAddress, ushort[] writeData);
	}
}
