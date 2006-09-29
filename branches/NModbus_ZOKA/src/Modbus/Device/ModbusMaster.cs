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
	internal class ModbusMaster
	{
		private ModbusTransport _transport;

		internal ModbusMaster(ModbusTransport transport)			
		{
			_transport = transport;
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
			ReadCoilsInputsResponse response = _transport.UnicastMessage<ReadCoilsInputsResponse>(request);

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
			ReadHoldingInputRegistersResponse response = _transport.UnicastMessage<ReadHoldingInputRegistersResponse>(request);
			
			return CollectionUtil.ToArray<ushort>(response.Data);
		}

		public void WriteSingleCoil(byte slaveAddress, ushort coilAddress, bool value)
		{
			WriteSingleCoilRequestResponse request = new WriteSingleCoilRequestResponse(slaveAddress, coilAddress, value);
			_transport.UnicastMessage<WriteSingleCoilRequestResponse>(request);
		}

		public void WriteSingleRegister(byte slaveAddress, ushort registerAddress, ushort value)
		{
			WriteSingleRegisterRequestResponse request = new WriteSingleRegisterRequestResponse(slaveAddress, registerAddress, value);
			_transport.UnicastMessage<WriteSingleRegisterRequestResponse>(request);
		}

		public void WriteMultipleRegisters(byte slaveAddress, ushort startAddress, ushort[] data)
		{
			WriteMultipleRegistersRequest request = new WriteMultipleRegistersRequest(slaveAddress, startAddress, new RegisterCollection(data));
			_transport.UnicastMessage<WriteMultipleRegistersResponse>(request);
		}

		public void WriteMultipleCoils(byte slaveAddress, ushort startAddress, bool[] data)
		{
			WriteMultipleCoilsRequest request = new WriteMultipleCoilsRequest(slaveAddress, startAddress, new DiscreteCollection(data));
			_transport.UnicastMessage<WriteMultipleCoilsResponse>(request);
		}
	}
}
