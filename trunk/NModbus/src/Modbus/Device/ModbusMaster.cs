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
	public abstract class ModbusMaster : ModbusDevice
	{
		public ModbusMaster(IModbusTransport transport)
			: base(transport)
		{
		}

		public bool[] ReadCoils(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			ReadCoilsRequest request = new ReadCoilsRequest(slaveAddress, startAddress, numberOfPoints);
			ReadCoilsResponse response = Transport.UnicastMessage<ReadCoilsResponse>(request);

			return CollectionUtil.Slice<bool>(response.Data, 0, request.NumberOfPoints);
		}

		public ushort[] ReadHoldingRegisters(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
		{
			ReadHoldingRegistersRequest request = new ReadHoldingRegistersRequest(slaveAddress, startAddress, numberOfPoints);
			ReadHoldingRegistersResponse response = Transport.UnicastMessage<ReadHoldingRegistersResponse>(request);
			
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
	}
}
