using System;
using System.Collections.Generic;
using System.Text;
using Modbus.IO;
using Modbus.Message;
using System.Collections;
using log4net;
using System.IO;
using Modbus.Util;
using Modbus.Data;

namespace Modbus.Device
{
	internal class ModbusMaster : ModbusDevice
	{
		public ModbusMaster(ModbusTransport transport)
			: base(transport)
		{
		}

		public bool[] ReadCoils(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			return ReadDiscretes(slaveAddress, startAddress, numberOfPoints);
		}

		public bool[] ReadInputs(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			return ReadDiscretes(slaveAddress, startAddress, numberOfPoints);
		}

		internal bool[] ReadDiscretes(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			ReadCoilsInputsRequest request = new ReadCoilsInputsRequest(Modbus.ReadCoils, slaveAddress, startAddress, numberOfPoints);
			ReadCoilsInputsResponse response = Transport.UnicastMessage<ReadCoilsInputsResponse>(request);

			return CollectionUtil.Slice<bool>(response.Data, 0, request.NumberOfPoints);
		}

		public ushort[] ReadHoldingRegisters(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			return ReadRegisters(slaveAddress, startAddress, numberOfPoints);
		}

		public ushort[] ReadInputRegisters(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			return ReadRegisters(slaveAddress, startAddress, numberOfPoints);
		}

		internal ushort[] ReadRegisters(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			ReadHoldingInputRegistersRequest request = new ReadHoldingInputRegistersRequest(Modbus.ReadHoldingRegisters, slaveAddress, startAddress, numberOfPoints);
			ReadHoldingInputRegistersResponse response = Transport.UnicastMessage<ReadHoldingInputRegistersResponse>(request);
			
			return CollectionUtil.ToArray<ushort>(response.Data);
		}

		public void WriteSingleCoil(byte slaveAddress, ushort coilAddress, bool value)
		{
			WriteSingleCoilRequestResponse request = new WriteSingleCoilRequestResponse(slaveAddress, coilAddress, value);
			WriteSingleCoilRequestResponse response = Transport.UnicastMessage<WriteSingleCoilRequestResponse>(request);
		}

		public void WriteSingleRegister(byte slaveAddress, ushort registerAddress, ushort value)
		{
			WriteSingleRegisterRequestResponse request = new WriteSingleRegisterRequestResponse(slaveAddress, registerAddress, value);
			WriteSingleRegisterRequestResponse response = Transport.UnicastMessage<WriteSingleRegisterRequestResponse>(request);
		}

		public void WriteMultipleRegisters(byte slaveAddress, ushort startAddress, ushort[] data)
		{
			WriteMultipleRegistersRequest request = new WriteMultipleRegistersRequest(slaveAddress, startAddress, new RegisterCollection(data));
			WriteMultipleRegistersResponse response = Transport.UnicastMessage<WriteMultipleRegistersResponse>(request);
		}

		public void WriteMultipleCoils(byte slaveAddress, ushort startAddress, bool[] data)
		{
			WriteMultipleCoilsRequest request = new WriteMultipleCoilsRequest(slaveAddress, startAddress, new DiscreteCollection(data));
			WriteMultipleCoilsResponse response = Transport.UnicastMessage<WriteMultipleCoilsResponse>(request);
		}
	}
}
