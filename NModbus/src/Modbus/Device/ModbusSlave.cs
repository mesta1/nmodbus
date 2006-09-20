using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using log4net;
using Modbus.IO;
using Modbus.Message;
using Modbus.Data;
using Modbus.Util;

namespace Modbus.Device
{
	public class ModbusSlave : ModbusDevice
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(ModbusSlave));
		private byte _unitID;
		private DataStore _dataStore;
		
		private ModbusSlave(byte unitID, ModbusTransport transport)
			: base(transport)
		{
			_dataStore = DataStoreFactory.CreateDefaultDataStore();
			_unitID = unitID;
		}

		public static ModbusSlave CreateAscii(byte unitID, SerialPort serialPort)
		{
			return new ModbusSlave(unitID, new ModbusAsciiTransport(serialPort));
		}

		public static ModbusSlave CreateRtu(byte unitID, SerialPort serialPort)
		{
			return new ModbusSlave(unitID, new ModbusRtuTransport(serialPort));
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
				try
				{
					// use transport to retrieve raw message frame from stream
					byte[] frame = Transport.Read();
					log.DebugFormat("RX: {0}", StringUtil.Join(", ", frame));

					// build request from frame
					IModbusMessage request = ModbusMessageFactory.CreateModbusRequest(frame);

					// only service requests addressed to this particular slave
					if (request.SlaveAddress != UnitID)
						continue;

					// perform action
					IModbusMessage response = ApplyRequest(request);

					// write response
					log.DebugFormat("TX: {0}", StringUtil.Join(", ", Transport.BuildMessageFrame(response)));
					Transport.Write(response);	

				}
				catch (Exception e)
				{
					log.ErrorFormat("Exception encountered while listening for requests - {0}", e.Message);
				}
			}
		}

		internal static ReadCoilsInputsResponse ReadDiscretes(ReadCoilsInputsRequest request, DiscreteCollection dataSource)
		{
			DiscreteCollection data = DataStore.ReadData<DiscreteCollection, bool>(dataSource, request.StartAddress, request.NumberOfPoints);
			ReadCoilsInputsResponse response = new ReadCoilsInputsResponse(request.SlaveAddress, data.ByteCount, data);

			return response;
		}

		internal static ReadHoldingInputRegistersResponse ReadRegisters(ReadHoldingInputRegistersRequest request, RegisterCollection dataSource)
		{
			RegisterCollection data = DataStore.ReadData<RegisterCollection, ushort>(dataSource, request.StartAddress, request.NumberOfPoints);
			ReadHoldingInputRegistersResponse response = new ReadHoldingInputRegistersResponse(request.SlaveAddress, data.ByteCount, data);

			return response;
		}

		internal static WriteSingleCoilRequestResponse WriteSingleCoil(WriteSingleCoilRequestResponse request, DiscreteCollection dataSource)
		{
			DataStore.WriteData<DiscreteCollection, bool>(new DiscreteCollection(request.Data[0] == Modbus.CoilOn), dataSource, request.StartAddress);
		
			return request;
		}

		internal static WriteMultipleCoilsResponse WriteMultipleCoils(WriteMultipleCoilsRequest request, DiscreteCollection dataSource)
		{
			DataStore.WriteData<DiscreteCollection, bool>(request.Data, dataSource, request.StartAddress);
			WriteMultipleCoilsResponse response = new WriteMultipleCoilsResponse(request.SlaveAddress, request.StartAddress, request.NumberOfPoints);

			return response;
		}

		internal IModbusMessage ApplyRequest(IModbusMessage request)
		{
			IModbusMessage response;

			switch (request.FunctionCode)
			{
				case Modbus.ReadCoils:
					response = ReadDiscretes((ReadCoilsInputsRequest) request, DataStore.CoilDiscretes);
					break;
				case Modbus.ReadInputs:
					response = ReadDiscretes((ReadCoilsInputsRequest) request, DataStore.InputDiscretes);
					break;
				case Modbus.ReadHoldingRegisters:
					response = ReadRegisters((ReadHoldingInputRegistersRequest) request, DataStore.HoldingRegisters);
					break;
				case Modbus.ReadInputRegisters:
					response = ReadRegisters((ReadHoldingInputRegistersRequest) request, DataStore.InputRegisters);
					break;
				case Modbus.WriteSingleCoil:
					response = WriteSingleCoil((WriteSingleCoilRequestResponse) request, DataStore.CoilDiscretes);
					break;
				//case Modbus.WriteSingleRegister:
				//    break;
				case Modbus.WriteMultipleCoils:
					response = WriteMultipleCoils((WriteMultipleCoilsRequest) request, DataStore.CoilDiscretes);
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
