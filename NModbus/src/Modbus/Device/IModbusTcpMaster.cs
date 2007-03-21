using System;
using System.Collections.Generic;
using System.Text;

namespace Modbus.Device
{
	public interface IModbusTcpMaster : IModbusMaster
	{
		/// <summary>
		/// Read from 1 to 2000 contiguous coils status.
		/// </summary>
		/// <param name="startAddress">Address to begin reading.</param>
		/// <param name="numberOfPoints">Number of coils to read.</param>
		/// <returns>Coils status</returns>
		bool[] ReadCoils(ushort startAddress, ushort numberOfPoints);

		/// <summary>
		/// Read from 1 to 2000 contiguous discrete input status.
		/// </summary>
		/// <param name="startAddress">Address to begin reading.</param>
		/// <param name="numberOfPoints">Number of discrete inputs to read.</param>
		/// <returns>Discrete inputs status</returns>
		bool[] ReadInputs(ushort startAddress, ushort numberOfPoints);

		/// <summary>
		/// Read contiguous block of holding registers.
		/// </summary>
		/// <param name="startAddress">Address to begin reading.</param>
		/// <param name="numberOfPoints">Number of holding registers to read.</param>
		/// <returns>Holding registers status</returns>
		ushort[] ReadHoldingRegisters(ushort startAddress, ushort numberOfPoints);

		/// <summary>
		/// Read contiguous block of input registers.
		/// </summary>
		/// <param name="startAddress">Address to begin reading.</param>
		/// <param name="numberOfPoints">Number of holding registers to read.</param>
		/// <returns>Input registers status</returns>
		ushort[] ReadInputRegisters(ushort startAddress, ushort numberOfPoints);

		/// <summary>
		/// Write a single coil value.
		/// </summary>
		/// <param name="coilAddress">Address to write value to.</param>
		/// <param name="value">Value to write.</param>
		void WriteSingleCoil(ushort coilAddress, bool value);

		/// <summary>
		/// Write a single holding register.
		/// </summary>
		/// <param name="registerAddress">Value to write.</param>
		/// <param name="value">Value to write.</param>
		void WriteSingleRegister(ushort registerAddress, ushort value);

		/// <summary>
		/// Write a block of 1 to 123 contiguous registers.
		/// </summary>
		/// <param name="startAddress">Address to begin writing values.</param>
		/// <param name="data">Values to write.</param>
		void WriteMultipleRegisters(ushort startAddress, ushort[] data);

		/// <summary>
		/// Force each coil in a sequence of coils to a provided value.
		/// </summary>
		/// <param name="startAddress">Address to begin writing values.</param>
		/// <param name="data">Values to write.</param>
		void WriteMultipleCoils(ushort startAddress, bool[] data);

		/// <summary>
		/// Performs a combination of one read operation and one write operation in a single Modbus transaction. 
		/// The write operation is performed before the read.
		/// </summary>
		/// <param name="startReadAddress">Address to begin reading (Holding registers are addressed starting at 0).</param>
		/// <param name="numberOfPointsToRead">Number of registers to read.</param>
		/// <param name="startWriteAddress">Address to begin writing (Holding registers are addressed starting at 0).</param>
		/// <param name="writeData">Register values to write.</param>
		ushort[] ReadWriteMultipleRegisters(ushort startReadAddress, ushort numberOfPointsToRead, ushort startWriteAddress, ushort[] writeData);
	}
}
