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

		public bool[] ReadCoils(ushort modbusAddress, ushort numberOfPoints)
		{
			return base.ReadCoils(Modbus.DefaultTcpSlaveUnitId, modbusAddress, numberOfPoints);
		}

		public bool[] ReadInputs(ushort modbusAddress, ushort numberOfPoints)
		{
			return base.ReadInputs(Modbus.DefaultTcpSlaveUnitId, modbusAddress, numberOfPoints);
		}

		public ushort[] ReadHoldingRegisters(ushort modbusAddress, ushort numberOfPoints)
		{
			return base.ReadHoldingRegisters(Modbus.DefaultTcpSlaveUnitId, modbusAddress, numberOfPoints);
		}

		public ushort[] ReadInputRegisters(ushort modbusAddress, ushort numberOfPoints)
		{
			return base.ReadHoldingRegisters(Modbus.DefaultTcpSlaveUnitId, modbusAddress, numberOfPoints);
		}

		public void WriteSingleCoil(ushort modbusAddress, bool value)
		{
			base.WriteSingleCoil(Modbus.DefaultTcpSlaveUnitId, modbusAddress, value);
		}

		public void WriteSingleRegister(ushort modbusAddress, ushort value)
		{
			base.WriteSingleRegister(Modbus.DefaultTcpSlaveUnitId, modbusAddress, value);
		}

		public void WriteMultipleRegisters(ushort modbusAddress, ushort[] data)
		{
			base.WriteMultipleRegisters(Modbus.DefaultTcpSlaveUnitId, modbusAddress, data);
		}

		public void WriteMultipleCoils(ushort modbusAddress, bool[] data)
		{
			base.WriteMultipleCoils(Modbus.DefaultTcpSlaveUnitId, modbusAddress, data);
		}
	}
}
