using System;
using System.Collections.Generic;
using System.Text;
using Modbus.IO;
using Modbus.Message;
using Modbus.Data;
using Modbus.Util;

namespace Modbus.Device
{
	internal class ModbusSlave : ModbusDevice
	{
		private byte _unitID;
		private DataStore _dataStore;

		public ModbusSlave(byte unitID, ModbusTransport transport, DataStore dataStore)
			: base(transport)
		{
			_dataStore = dataStore;
			_unitID = unitID;
		}

		public DataStore DataStore
		{
			get { return _dataStore; }
			set { _dataStore = value; }
		}

		public byte UnitID
		{
			get { return _unitID; }
			set { _unitID = value; }
		}
	
		public void Listen()
		{
			while (true)
			{
				// use transport to retrieve raw message frame from stream
				byte[] frame = Transport.Read();

				// build request from frame
				IModbusMessage request = ModbusMessageFactory.CreateModbusRequest(frame);

				// check unit id

				// perform action
				IModbusMessage response = ApplyRequest(request);

				// write response
				Transport.Write(response);	
			}
		}

		public static ReadCoilsInputsResponse ReadDiscretes(ReadCoilsInputsRequest request, byte unitID, DiscreteCollection dataSource)
		{
			DiscreteCollection data = DataStore.ReadData<DiscreteCollection, bool>(dataSource, request.StartAddress, request.NumberOfPoints);
			ReadCoilsInputsResponse response = new ReadCoilsInputsResponse(unitID, data.ByteCount, data);

			return response;
		}

		public static ReadHoldingInputRegistersResponse ReadRegisters(ReadHoldingInputRegistersRequest request, byte unitID, RegisterCollection dataSource)
		{
			RegisterCollection data = DataStore.ReadData<RegisterCollection, ushort>(dataSource, request.StartAddress, request.NumberOfPoints);
			ReadHoldingInputRegistersResponse response = new ReadHoldingInputRegistersResponse(unitID, data.ByteCount, data);

			return response;
		}

		public static WriteSingleCoilRequestResponse WriteSingleCoil(WriteSingleCoilRequestResponse request, byte unitID, DiscreteCollection dataSource)
		{
			DataStore.WriteData<DiscreteCollection, bool>(new DiscreteCollection(request.Data[0] == Modbus.CoilOn), dataSource, request.StartAddress);
		
			return request;
		}

		public static WriteMultipleCoilsResponse WriteMultipleCoils(WriteMultipleCoilsRequest request, byte unitID, DiscreteCollection dataSource)
		{
			DataStore.WriteData<DiscreteCollection, bool>(request.Data, dataSource, request.StartAddress);
			WriteMultipleCoilsResponse response = new WriteMultipleCoilsResponse(unitID, request.StartAddress, request.NumberOfPoints);

			return response;
		}

		public IModbusMessage ApplyRequest(IModbusMessage request)
		{
			IModbusMessage response;

			switch (request.FunctionCode)
			{
				case Modbus.ReadCoils:
					response = ReadDiscretes((ReadCoilsInputsRequest) request, UnitID, DataStore.CoilDiscretes);
					break;
				case Modbus.ReadInputs:
					response = ReadDiscretes((ReadCoilsInputsRequest) request, UnitID, DataStore.InputDiscretes);
					break;
				case Modbus.ReadHoldingRegisters:
					response = ReadRegisters((ReadHoldingInputRegistersRequest) request, UnitID, DataStore.HoldingRegisters);
					break;
				case Modbus.ReadInputRegisters:
					response = ReadRegisters((ReadHoldingInputRegistersRequest) request, UnitID, DataStore.InputRegisters);
					break;
				case Modbus.WriteSingleCoil:
					response = WriteSingleCoil((WriteSingleCoilRequestResponse) request, UnitID, DataStore.CoilDiscretes);
					break;
				//case Modbus.WriteSingleRegister:
				//    break;
				case Modbus.WriteMultipleCoils:
					response = WriteMultipleCoils((WriteMultipleCoilsRequest) request, UnitID, DataStore.CoilDiscretes);
					break;
				//case Modbus.WriteMultipleRegisters:
				//    break;
				default:
					throw new ArgumentException(String.Format("Unsupported function code {0}", request.FunctionCode), "request");
			}

			return response;
		}
	}
}
