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
			return base.ReadCoils(Modbus.DefaultTcpSlaveUnitID, startAddress, numberOfPoints);
		}

		public bool[] ReadInputs(ushort startAddress, ushort numberOfPoints)
		{
			return base.ReadInputs(Modbus.DefaultTcpSlaveUnitID, startAddress, numberOfPoints);
		}

		public ushort[] ReadHoldingRegisters(ushort startAddress, ushort numberOfPoints)
		{
			return base.ReadHoldingRegisters(Modbus.DefaultTcpSlaveUnitID, startAddress, numberOfPoints);
		}

		public ushort[] ReadInputRegisters(ushort startAddress, ushort numberOfPoints)
		{
			return base.ReadHoldingRegisters(Modbus.DefaultTcpSlaveUnitID, startAddress, numberOfPoints);
		}

		public void WriteSingleCoil(ushort coilAddress, bool value)
		{
			base.WriteSingleCoil(Modbus.DefaultTcpSlaveUnitID, coilAddress, value);
		}

		public void WriteSingleRegister(ushort registerAddress, ushort value)
		{
			base.WriteSingleRegister(Modbus.DefaultTcpSlaveUnitID, registerAddress, value);
		}

		public void WriteMultipleRegisters(ushort startAddress, ushort[] data)
		{
			base.WriteMultipleRegisters(Modbus.DefaultTcpSlaveUnitID, startAddress, data);
		}

		public void WriteMultipleCoils(ushort startAddress, bool[] data)
		{
			base.WriteMultipleCoils(Modbus.DefaultTcpSlaveUnitID, startAddress, data);
		}
	}
}
