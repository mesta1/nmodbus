using System;
using System.Collections.Generic;
using System.Text;
using Modbus.IO;
using System.Net.Sockets;

namespace Modbus.Device
{
	/// <summary>
	/// Modbus IP based TCP master.
	/// </summary>
	public class ModbusTcpMaster : ModbusMaster, IModbusTcpMaster
	{
		private ModbusTcpMaster(ModbusTcpTransport transport)
			: base(transport)
		{
		}

		public static ModbusTcpMaster CreateTcp(TcpClient tcpClient)
		{
			return new ModbusTcpMaster(new ModbusTcpTransport(tcpClient));
		}

		public bool[] ReadCoils(ushort startAddress, ushort numberOfPoints)
		{
			return base.ReadCoils(Modbus.DefaultTcpSlaveUnitId, startAddress, numberOfPoints);
		}

		public bool[] ReadInputs(ushort startAddress, ushort numberOfPoints)
		{
			return base.ReadInputs(Modbus.DefaultTcpSlaveUnitId, startAddress, numberOfPoints);
		}

		public ushort[] ReadHoldingRegisters(ushort startAddress, ushort numberOfPoints)
		{
			return base.ReadHoldingRegisters(Modbus.DefaultTcpSlaveUnitId, startAddress, numberOfPoints);
		}

		public ushort[] ReadInputRegisters(ushort startAddress, ushort numberOfPoints)
		{
			return base.ReadHoldingRegisters(Modbus.DefaultTcpSlaveUnitId, startAddress, numberOfPoints);
		}

		public void WriteSingleCoil(ushort coilAddress, bool value)
		{
			base.WriteSingleCoil(Modbus.DefaultTcpSlaveUnitId, coilAddress, value);
		}

		public void WriteSingleRegister(ushort registerAddress, ushort value)
		{
			base.WriteSingleRegister(Modbus.DefaultTcpSlaveUnitId, registerAddress, value);
		}

		public void WriteMultipleRegisters(ushort startAddress, ushort[] data)
		{
			base.WriteMultipleRegisters(Modbus.DefaultTcpSlaveUnitId, startAddress, data);
		}

		public void WriteMultipleCoils(ushort startAddress, bool[] data)
		{
			base.WriteMultipleCoils(Modbus.DefaultTcpSlaveUnitId, startAddress, data);
		}

		/// <summary>
		/// Performs a combination of one read operation and one write operation in a single MODBUS transaction. 
		/// The write operation is performed before the read.
		/// Message uses default TCP slave id of 0.
		/// </summary>
		/// <param name="startReadAddress">Address to begin reading (Holding registers are addressed starting at 0).</param>
		/// <param name="numberOfPointsToRead">Number of registers to read.</param>
		/// <param name="startWriteAddress">Address to begin writing (Holding registers are addressed starting at 0).</param>
		/// <param name="numberOfPointsToWrite">Number of registers to write.</param>
		/// <param name="writeData">Register values to write.</param>
		public ushort[] ReadWriteMultipleRegisters(ushort startReadAddress, ushort numberOfPointsToRead, ushort startWriteAddress, ushort numberOfPointsToWrite, ushort[] writeData)
		{
			return base.ReadWriteMultipleRegisters(Modbus.DefaultTcpSlaveUnitId, startReadAddress, numberOfPointsToRead, startWriteAddress, numberOfPointsToWrite, writeData);
		}
	}
}
