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

		public bool[] ReadCoils(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			return ReadDiscretes(Modbus.ReadCoils, slaveAddress, startAddress, numberOfPoints);
		}
			
		public bool[] ReadInputs(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			return ReadDiscretes(Modbus.ReadInputs, slaveAddress, startAddress, numberOfPoints);
		}		

		public ushort[] ReadHoldingRegisters(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			return ReadRegisters(Modbus.ReadHoldingRegisters, slaveAddress, startAddress, numberOfPoints);
		}

		public ushort[] ReadInputRegisters(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			return ReadRegisters(Modbus.ReadInputRegisters, slaveAddress, startAddress, numberOfPoints);
		}

		public void WriteSingleCoil(byte slaveAddress, ushort coilAddress, bool value)
		{
			WriteSingleCoilRequestResponse request = new WriteSingleCoilRequestResponse(slaveAddress, coilAddress, value);
			Transport.UnicastMessage<WriteSingleCoilRequestResponse>(request);
		}

		public void WriteSingleRegister(byte slaveAddress, ushort registerAddress, ushort value)
		{
			WriteSingleRegisterRequestResponse request = new WriteSingleRegisterRequestResponse(slaveAddress, registerAddress, value);
			Transport.UnicastMessage<WriteSingleRegisterRequestResponse>(request);
		}

		public void WriteMultipleRegisters(byte slaveAddress, ushort startAddress, ushort[] data)
		{
			WriteMultipleRegistersRequest request = new WriteMultipleRegistersRequest(slaveAddress, startAddress, new RegisterCollection(data));
			Transport.UnicastMessage<WriteMultipleRegistersResponse>(request);
		}

		public void WriteMultipleCoils(byte slaveAddress, ushort startAddress, bool[] data)
		{
			WriteMultipleCoilsRequest request = new WriteMultipleCoilsRequest(slaveAddress, startAddress, new DiscreteCollection(data));
			Transport.UnicastMessage<WriteMultipleCoilsResponse>(request);
		}

		/// <summary>
		/// Performs a combination of one read operation and one write operation in a single Modbus transaction. 
		/// The write operation is performed before the read.
		/// </summary>
		/// <param name="slaveAddress">Address of device to read values from.</param>
		/// <param name="startReadAddress">Address to begin reading (Holding registers are addressed starting at 0).</param>
		/// <param name="numberOfPointsToRead">Number of registers to read.</param>
		/// <param name="startWriteAddress">Address to begin writing (Holding registers are addressed starting at 0).</param>
		/// <param name="writeData">Register values to write.</param>
		public ushort[] ReadWriteMultipleRegisters(byte slaveAddress, ushort startReadAddress, ushort numberOfPointsToRead, ushort startWriteAddress, ushort[] writeData)
		{
			ReadWriteMultipleRegistersRequest request = new ReadWriteMultipleRegistersRequest(slaveAddress, startReadAddress, numberOfPointsToRead, startWriteAddress, new RegisterCollection(writeData));			
			ReadHoldingInputRegistersResponse response = Transport.UnicastMessage<ReadHoldingInputRegistersResponse>(request);

			return CollectionUtil.ToArray<ushort>(response.Data);
		}

		internal ushort[] ReadRegisters(byte functionCode, byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			ReadHoldingInputRegistersRequest request = new ReadHoldingInputRegistersRequest(functionCode, slaveAddress, startAddress, numberOfPoints);
			ReadHoldingInputRegistersResponse response = Transport.UnicastMessage<ReadHoldingInputRegistersResponse>(request);

			return CollectionUtil.ToArray<ushort>(response.Data);
		}

		internal bool[] ReadDiscretes(byte functionCode, byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			ReadCoilsInputsRequest request = new ReadCoilsInputsRequest(functionCode, slaveAddress, startAddress, numberOfPoints);
			ReadCoilsInputsResponse response = Transport.UnicastMessage<ReadCoilsInputsResponse>(request);

			return CollectionUtil.Slice<bool>(response.Data, 0, request.NumberOfPoints);
		}
	}
}
