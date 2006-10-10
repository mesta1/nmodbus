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
	public class ModbusTcpMaster : ModbusDevice, IModbusTcpMaster, IModbusMaster
	{
		private ModbusMaster _modbusMasterImpl;		

		private ModbusTcpMaster(ModbusTcpTransport transport)
			: base(transport)
		{
			_modbusMasterImpl = new ModbusMaster(Transport);
		}

		public static ModbusTcpMaster CreateTcp(TcpClient tcpClient)
		{
			return new ModbusTcpMaster(new ModbusTcpTransport(tcpClient));
		}

		public bool[] ReadCoils(ushort startAddress, ushort numberOfPoints)
		{
			return _modbusMasterImpl.ReadCoils(0, startAddress, numberOfPoints);
		}

		public bool[] ReadInputs(ushort startAddress, ushort numberOfPoints)
		{
			return _modbusMasterImpl.ReadInputs(0, startAddress, numberOfPoints);
		}

		public ushort[] ReadHoldingRegisters(ushort startAddress, ushort numberOfPoints)
		{
			return _modbusMasterImpl.ReadHoldingRegisters(0, startAddress, numberOfPoints);
		}

		public ushort[] ReadInputRegisters(ushort startAddress, ushort numberOfPoints)
		{
			return _modbusMasterImpl.ReadHoldingRegisters(0, startAddress, numberOfPoints);
		}

		public void WriteSingleCoil(ushort coilAddress, bool value)
		{
			_modbusMasterImpl.WriteSingleCoil(0, coilAddress, value);
		}

		public void WriteSingleRegister(ushort registerAddress, ushort value)
		{
			_modbusMasterImpl.WriteSingleRegister(0, registerAddress, value);
		}

		public void WriteMultipleRegisters(ushort startAddress, ushort[] data)
		{
			_modbusMasterImpl.WriteMultipleRegisters(0, startAddress, data);
		}

		public void WriteMultipleCoils(ushort startAddress, bool[] data)
		{
			_modbusMasterImpl.WriteMultipleCoils(0, startAddress, data);
		}

		#region IModbusMaster Members

		bool[] IModbusMaster.ReadCoils(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			return ReadCoils(startAddress, numberOfPoints);
		}

		bool[] IModbusMaster.ReadInputs(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			return ReadInputs(startAddress, numberOfPoints);
		}

		ushort[] IModbusMaster.ReadHoldingRegisters(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			return ReadHoldingRegisters(startAddress, numberOfPoints);
		}

		ushort[] IModbusMaster.ReadInputRegisters(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			return ReadHoldingRegisters(startAddress, numberOfPoints);
		}

		void IModbusMaster.WriteSingleCoil(byte slaveAddress, ushort coilAddress, bool value)
		{
			WriteSingleCoil(coilAddress, value);
		}

		void IModbusMaster.WriteSingleRegister(byte slaveAddress, ushort registerAddress, ushort value)
		{
			WriteSingleRegister(registerAddress, value);
		}

		void IModbusMaster.WriteMultipleRegisters(byte slaveAddress, ushort startAddress, ushort[] data)
		{
			WriteMultipleRegisters(startAddress, data);
		}

		void IModbusMaster.WriteMultipleCoils(byte slaveAddress, ushort startAddress, bool[] data)
		{
			WriteMultipleCoils(startAddress, data);
		}

		#endregion
	}
}
