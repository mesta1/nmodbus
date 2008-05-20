using System;
using log4net;
using Modbus.Data;
using Modbus.IO;
using Modbus.Message;
using Unme.Common.NullReferenceExtension;

namespace Modbus.Device
{
	/// <summary>
	/// Represents the method that will handle the ModbusSlaveRequestReceived event of a ModbusSlave object.
	/// </summary>
	public delegate void ModbusSlaveRequestReceivedEventHandler(object sender, ModbusSlaveRequestEventArgs e);

	/// <summary>
	/// Modbus slave device.
	/// </summary>
	public abstract class ModbusSlave : ModbusDevice
	{
		/// <summary>
		/// Occurs when a modbus slave receives a request.
		/// </summary>
		public event ModbusSlaveRequestReceivedEventHandler ModbusSlaveRequestReceived;

		private static readonly ILog _log = LogManager.GetLogger(typeof(ModbusSlave));

		internal ModbusSlave(byte unitID, ModbusTransport transport)
			: base(transport)
		{
			DataStore = DataStoreFactory.CreateDefaultDataStore();
			UnitID = unitID;
		}

		/// <summary>
		/// Gets or sets the data store.
		/// </summary>
		public DataStore DataStore { get; set; }
	
		/// <summary>
		/// Gets or sets the unit ID.
		/// </summary>
		public byte UnitID { get; set; }

		/// <summary>
		/// Start slave listening for requests.
		/// </summary>
		public abstract void Listen();
		
		internal static ReadCoilsInputsResponse ReadDiscretes(ReadCoilsInputsRequest request, DataStore dataStore, ModbusDataCollection<bool> dataSource)
		{
			DiscreteCollection data = DataStore.ReadData<DiscreteCollection, bool>(dataStore, dataSource, request.StartAddress, request.NumberOfPoints, dataStore.SyncRoot);
			ReadCoilsInputsResponse response = new ReadCoilsInputsResponse(request.FunctionCode, request.SlaveAddress, data.ByteCount, data);

			return response;
		}

		internal static ReadHoldingInputRegistersResponse ReadRegisters(ReadHoldingInputRegistersRequest request, DataStore dataStore, ModbusDataCollection<ushort> dataSource)
		{
			RegisterCollection data = DataStore.ReadData<RegisterCollection, ushort>(dataStore, dataSource, request.StartAddress, request.NumberOfPoints, dataStore.SyncRoot);
			ReadHoldingInputRegistersResponse response = new ReadHoldingInputRegistersResponse(request.FunctionCode, request.SlaveAddress, data);

			return response;
		}

		internal static WriteSingleCoilRequestResponse WriteSingleCoil(WriteSingleCoilRequestResponse request, DataStore dataStore, ModbusDataCollection<bool> dataSource)
		{
			DataStore.WriteData(dataStore, new DiscreteCollection(request.Data[0] == Modbus.CoilOn), dataSource, request.StartAddress, dataStore.SyncRoot);
		
			return request;
		}

		internal static WriteMultipleCoilsResponse WriteMultipleCoils(WriteMultipleCoilsRequest request, DataStore dataStore, ModbusDataCollection<bool> dataSource)
		{
			DataStore.WriteData(dataStore, request.Data, dataSource, request.StartAddress, dataStore.SyncRoot);
			WriteMultipleCoilsResponse response = new WriteMultipleCoilsResponse(request.SlaveAddress, request.StartAddress, request.NumberOfPoints);

			return response;
		}

		internal static WriteSingleRegisterRequestResponse WriteSingleRegister(WriteSingleRegisterRequestResponse request, DataStore dataStore, ModbusDataCollection<ushort> dataSource)
		{
			DataStore.WriteData(dataStore, request.Data, dataSource, request.StartAddress, dataStore.SyncRoot);
			
			return request;
		}

		internal static WriteMultipleRegistersResponse WriteMultipleRegisters(WriteMultipleRegistersRequest request, DataStore dataStore, ModbusDataCollection<ushort> dataSource)
		{
			DataStore.WriteData(dataStore, request.Data, dataSource, request.StartAddress, dataStore.SyncRoot);
			WriteMultipleRegistersResponse response = new WriteMultipleRegistersResponse(request.SlaveAddress, request.StartAddress, request.NumberOfPoints);

			return response;
		}

		internal IModbusMessage ApplyRequest(IModbusMessage request)
		{			
			_log.Info(request.ToString());
			ModbusSlaveRequestReceived.IfNotNull((e) => e(this, new ModbusSlaveRequestEventArgs(request)));

			IModbusMessage response;
			switch (request.FunctionCode)
			{
				case Modbus.ReadCoils:
					response = ReadDiscretes((ReadCoilsInputsRequest) request, DataStore, DataStore.CoilDiscretes);
					break;
				case Modbus.ReadInputs:
					response = ReadDiscretes((ReadCoilsInputsRequest) request, DataStore, DataStore.InputDiscretes);
					break;
				case Modbus.ReadHoldingRegisters:
					response = ReadRegisters((ReadHoldingInputRegistersRequest) request, DataStore, DataStore.HoldingRegisters);
					break;
				case Modbus.ReadInputRegisters:
					response = ReadRegisters((ReadHoldingInputRegistersRequest) request, DataStore, DataStore.InputRegisters);
					break;
				case Modbus.Diagnostics:
					response = request;
					break;
				case Modbus.WriteSingleCoil:
					response = WriteSingleCoil((WriteSingleCoilRequestResponse) request, DataStore, DataStore.CoilDiscretes);
					break;
				case Modbus.WriteSingleRegister:
					response = WriteSingleRegister((WriteSingleRegisterRequestResponse) request, DataStore, DataStore.HoldingRegisters);
					break;
				case Modbus.WriteMultipleCoils:
					response = WriteMultipleCoils((WriteMultipleCoilsRequest) request, DataStore, DataStore.CoilDiscretes);
					break;
				case Modbus.WriteMultipleRegisters:
					response = WriteMultipleRegisters((WriteMultipleRegistersRequest) request, DataStore, DataStore.HoldingRegisters);
					break;
				case Modbus.ReadWriteMultipleRegisters:
					ReadWriteMultipleRegistersRequest readWriteRequest = (ReadWriteMultipleRegistersRequest) request;
					response = ReadRegisters(readWriteRequest.ReadRequest, DataStore, DataStore.HoldingRegisters);
					WriteMultipleRegisters(readWriteRequest.WriteRequest, DataStore, DataStore.HoldingRegisters);
					break;
				default:
					string errorMessage = String.Format("Unsupported function code {0}", request.FunctionCode);
					_log.Error(errorMessage);
					throw new ArgumentException(errorMessage, "request");
			}			

			return response;
		}
	}
}
