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

				// perform action
				IModbusMessage response = ApplyRequest(request);

				// write response
				Transport.Write(response);				
			}
		}

		public static ReadCoilsResponse ReadCoils(IModbusMessage message, byte unitID, DataStore dataStore)
		{
			ReadCoilsInputsRequest request = message as ReadCoilsInputsRequest;

			if (request == null)
				throw new ArgumentException("Invalid type.", "message");

			// get the data and build the response
			DiscreteCollection data = new DiscreteCollection(CollectionUtil.Slice<bool>(dataStore.CoilDiscretes, request.StartAddress, request.NumberOfPoints));
			ReadCoilsResponse response = new ReadCoilsResponse(unitID, 1, data);

			return response;
		}

		public IModbusMessage ReadInputs(IModbusMessage message, byte unitID, DataStore dataStore)
		{
			ReadHoldingInputRegistersRequest request = message as ReadHoldingInputRegistersRequest;

			if (request == null)
				throw new ArgumentException("Invalid type.", "message");

			return null;
		}

		public IModbusMessage ApplyRequest(IModbusMessage request)
		{
			IModbusMessage response;

			switch (request.FunctionCode)
			{
				case Modbus.ReadCoils:
					response = ReadCoils(request, UnitID, DataStore);
					break;
				case Modbus.ReadInputs:
					response = ReadInputs(request, UnitID, DataStore);
					break;
				//case Modbus.ReadHoldingRegisters:
				//    break;
				//case Modbus.ReadInputRegisters:
				//    break;
				//case Modbus.WriteSingleCoil:
				//    break;
				//case Modbus.WriteSingleRegister:
				//    break;
				//case Modbus.WriteMultipleCoils:
				//    break;
				//case Modbus.WriteMultipleRegisters:
				//    break;
				default:
					throw new ArgumentException(String.Format("Unsupported function code {0}", request.FunctionCode), "request");
			}

			return response;
		}
	}
}
