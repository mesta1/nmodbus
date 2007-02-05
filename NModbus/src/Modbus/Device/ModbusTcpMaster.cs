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
	}
}
