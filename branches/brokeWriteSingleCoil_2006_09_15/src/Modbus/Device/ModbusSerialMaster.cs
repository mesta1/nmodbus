using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using Modbus.IO;

namespace Modbus.Device
{
	/// <summary>
	/// Modbus serial ASCII master.
	/// </summary>
	public class ModbusSerialMaster : IModbusMaster
	{
		private ModbusMaster _modbusMasterImpl;

		private ModbusSerialMaster(ModbusTransport transport)
		{
			_modbusMasterImpl = new ModbusMaster(transport);
		}

		public static ModbusSerialMaster CreateAscii(SerialPort serialPort)
		{
			return new ModbusSerialMaster(new ModbusAsciiTransport(serialPort));
		}

		public static ModbusSerialMaster CreateRtu(SerialPort serialPort)
		{
			return new ModbusSerialMaster(new ModbusRtuTransport(serialPort));
		}

		public bool[] ReadCoils(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			return _modbusMasterImpl.ReadCoils(slaveAddress, startAddress, numberOfPoints);
		}

		public bool[] ReadInputs(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			return _modbusMasterImpl.ReadInputs(slaveAddress, startAddress, numberOfPoints);
		}

		public ushort[] ReadHoldingRegisters(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			return _modbusMasterImpl.ReadHoldingRegisters(slaveAddress, startAddress, numberOfPoints);
		}

		public void WriteSingleCoil(byte slaveAddress, ushort coilAddress, bool value)
		{
			_modbusMasterImpl.WriteSingleCoil(slaveAddress, coilAddress, value);
		}

		public void WriteSingleRegister(byte slaveAddress, ushort registerAddress, ushort value)
		{
			_modbusMasterImpl.WriteSingleRegister(slaveAddress, registerAddress, value);
		}

		public void WriteMultipleRegisters(byte slaveAddress, ushort startAddress, ushort[] data)
		{
			_modbusMasterImpl.WriteMultipleRegisters(slaveAddress, startAddress, data);
		}

		public void WriteMultipleCoils(byte slaveAddress, ushort startAddress, bool[] data)
		{
			_modbusMasterImpl.WriteMultipleCoils(slaveAddress, startAddress, data);
		}
	}
}
