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
	public class ModbusTCPMaster1 : IModbusTcpMaster, IModbusSerialMaster
	{
		private ModbusMaster _modbusMasterImpl;

		private ModbusTCPMaster1(ModbusTCPTransport1 transport)
		{
			_modbusMasterImpl = new ModbusMaster(transport);
		}

		public static ModbusTCPMaster1 CreateTcp(Socket socket)
		{
			return new ModbusTCPMaster1(new ModbusTCPTransport1(socket));
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

		#region IModbusSerialMaster Members

		bool[] IModbusSerialMaster.ReadCoils(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			return ReadCoils(startAddress, numberOfPoints);
		}

		bool[] IModbusSerialMaster.ReadInputs(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			return ReadInputs(startAddress, numberOfPoints);
		}

		ushort[] IModbusSerialMaster.ReadHoldingRegisters(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			return ReadHoldingRegisters(startAddress, numberOfPoints);
		}

		void IModbusSerialMaster.WriteSingleCoil(byte slaveAddress, ushort coilAddress, bool value)
		{
			WriteSingleCoil(coilAddress, value);
		}

		void IModbusSerialMaster.WriteSingleRegister(byte slaveAddress, ushort registerAddress, ushort value)
		{
			WriteSingleRegister(registerAddress, value);
		}

		void IModbusSerialMaster.WriteMultipleRegisters(byte slaveAddress, ushort startAddress, ushort[] data)
		{
			WriteMultipleRegisters(startAddress, data);
		}

		void IModbusSerialMaster.WriteMultipleCoils(byte slaveAddress, ushort startAddress, bool[] data)
		{
			WriteMultipleCoils(startAddress, data);
		}

		#endregion
	}
}
