using System;
using System.Collections.Generic;
using System.Text;
using Modbus.IO;
using Modbus.Message;
using System.Collections;
using System.IO;
using Modbus.Util;
using Modbus.Data;

namespace Modbus.Device
{
	public abstract class ModbusMaster : ModbusDevice
	{
		public ModbusMaster(ModbusTransport transport)
			: base(transport)
		{
		}

		public bool[] ReadCoils(byte slaveAddress, ushort modbusAddress, ushort numberOfPoints)
		{
			return ReadDiscretes(Modbus.ReadCoils, slaveAddress, modbusAddress, numberOfPoints);
		}

		public bool[] ReadInputs(byte slaveAddress, ushort modbusAddress, ushort numberOfPoints)
		{
			return ReadDiscretes(Modbus.ReadInputs, slaveAddress, modbusAddress, numberOfPoints);
		}

		public ushort[] ReadHoldingRegisters(byte slaveAddress, ushort modbusAddress, ushort numberOfPoints)
		{
			return ReadRegisters(Modbus.ReadHoldingRegisters, slaveAddress, modbusAddress, numberOfPoints);
		}

		public ushort[] ReadInputRegisters(byte slaveAddress, ushort modbusAddress, ushort numberOfPoints)
		{
			return ReadRegisters(Modbus.ReadInputRegisters, slaveAddress, modbusAddress, numberOfPoints);
		}

		public void WriteSingleCoil(byte slaveAddress, ushort modbusAddress, bool value)
		{
			WriteSingleCoilRequestResponse request = new WriteSingleCoilRequestResponse(slaveAddress, modbusAddress, value);
			Transport.UnicastMessage<WriteSingleCoilRequestResponse>(request);
		}

		public void WriteSingleRegister(byte slaveAddress, ushort modbusAddress, ushort value)
		{
			WriteSingleRegisterRequestResponse request = new WriteSingleRegisterRequestResponse(slaveAddress, modbusAddress, value);
			Transport.UnicastMessage<WriteSingleRegisterRequestResponse>(request);
		}

		public void WriteMultipleRegisters(byte slaveAddress, ushort modbusAddress, ushort[] data)
		{
			WriteMultipleRegistersRequest request = new WriteMultipleRegistersRequest(slaveAddress, modbusAddress, new RegisterCollection(data));
			Transport.UnicastMessage<WriteMultipleRegistersResponse>(request);
		}

		public void WriteMultipleCoils(byte slaveAddress, ushort modbusAddress, bool[] data)
		{
			WriteMultipleCoilsRequest request = new WriteMultipleCoilsRequest(slaveAddress, modbusAddress, new DiscreteCollection(data));
			Transport.UnicastMessage<WriteMultipleCoilsResponse>(request);
		}

		internal ushort[] ReadRegisters(byte functionCode, byte slaveAddress, ushort modbusAddress, ushort numberOfPoints)
		{
			ReadHoldingInputRegistersRequest request = new ReadHoldingInputRegistersRequest(functionCode, slaveAddress, modbusAddress, numberOfPoints);
			ReadHoldingInputRegistersResponse response = Transport.UnicastMessage<ReadHoldingInputRegistersResponse>(request);

			return CollectionUtil.ToArray<ushort>(response.Data);
		}

		internal bool[] ReadDiscretes(byte functionCode, byte slaveAddress, ushort modbusAddress, ushort numberOfPoints)
		{
			ReadCoilsInputsRequest request = new ReadCoilsInputsRequest(functionCode, slaveAddress, modbusAddress, numberOfPoints);
			ReadCoilsInputsResponse response = Transport.UnicastMessage<ReadCoilsInputsResponse>(request);

			return CollectionUtil.Slice<bool>(response.Data, 0, request.NumberOfPoints);
		}
	}
}
