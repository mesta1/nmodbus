using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Device;
using Modbus.Data;
using Modbus.Message;
using Modbus.UnitTests.Message;
using Modbus.Util;

namespace Modbus.UnitTests.Device
{
	[TestFixture]
	public class ModbusSlaveFixture : ModbusMessageFixture
	{
		private DataStore _testDataStore;

		[SetUp]
		public void SetUp()
		{
			_testDataStore = DataStoreFactory.CreateTestDataStore();
		}

		[Test]
		public void ReadDiscretesCoils()
		{
			ReadCoilsInputsResponse expectedResponse = new ReadCoilsInputsResponse(1, 2, new DiscreteCollection(false, true, false, true, false, true, false, true, false));
			ReadCoilsInputsResponse response = ModbusSlave.ReadDiscretes(new ReadCoilsInputsRequest(Modbus.ReadCoils, 1, 1, 9), 1, _testDataStore.CoilDiscretes);
			AssertModbusMessagePropertiesAreEqual(expectedResponse, response);
			Assert.AreEqual(expectedResponse.ByteCount, response.ByteCount);
		}

		[Test]
		public void ReadDiscretesInputs()
		{ 
			ReadCoilsInputsResponse expectedResponse = new ReadCoilsInputsResponse(1, 2, new DiscreteCollection(true, false, true, false, true, false, true, false, true));
			ReadCoilsInputsResponse response = ModbusSlave.ReadDiscretes(new ReadCoilsInputsRequest(Modbus.ReadInputs, 1, 1, 9), 1, _testDataStore.InputDiscretes);
			AssertModbusMessagePropertiesAreEqual(expectedResponse, response);
			Assert.AreEqual(expectedResponse.ByteCount, response.ByteCount);
		}

		[Test]
		public void ReadRegistersHoldingRegisters()
		{
			ReadHoldingInputRegistersResponse expectedResponse = new ReadHoldingInputRegistersResponse(1, 12, new RegisterCollection(1, 2, 3, 4, 5, 6));
			ReadHoldingInputRegistersResponse response = ModbusSlave.ReadRegisters(new ReadHoldingInputRegistersRequest(Modbus.ReadHoldingRegisters, 1, 1, 6), 1, _testDataStore.HoldingRegisters);
			AssertModbusMessagePropertiesAreEqual(expectedResponse, response);
			Assert.AreEqual(expectedResponse.ByteCount, response.ByteCount);
		}

		[Test]
		public void ReadRegistersInputRegisters()
		{
			ReadHoldingInputRegistersResponse expectedResponse = new ReadHoldingInputRegistersResponse(1, 12, new RegisterCollection(10, 20, 30, 40, 50, 60));
			ReadHoldingInputRegistersResponse response = ModbusSlave.ReadRegisters(new ReadHoldingInputRegistersRequest(Modbus.ReadInputRegisters, 1, 1, 6), 1, _testDataStore.InputRegisters);
			AssertModbusMessagePropertiesAreEqual(expectedResponse, response);
			Assert.AreEqual(expectedResponse.ByteCount, response.ByteCount);
		}

		[Test]
		public void WriteSingleCoil()
		{			
			ushort addressToWrite = 35;
			bool valueToWrite = !_testDataStore.CoilDiscretes[addressToWrite - 1];
			WriteSingleCoilRequestResponse expectedResponse = new WriteSingleCoilRequestResponse(1, addressToWrite, valueToWrite);
			WriteSingleCoilRequestResponse response = ModbusSlave.WriteSingleCoil(new WriteSingleCoilRequestResponse(1, addressToWrite, valueToWrite), 1, _testDataStore.CoilDiscretes);
			AssertModbusMessagePropertiesAreEqual(expectedResponse, response);
			Assert.AreEqual(valueToWrite, _testDataStore.CoilDiscretes[addressToWrite - 1]);
		}

		[Test]
		public void WriteMultipleCoils()
		{
			ushort startAddress = 35;
			ushort numberOfPoints = 10;
			bool val = !_testDataStore.CoilDiscretes[startAddress - 1];
			WriteMultipleCoilsResponse expectedResponse = new WriteMultipleCoilsResponse(1, startAddress, numberOfPoints);
			WriteMultipleCoilsResponse response = ModbusSlave.WriteMultipleCoils(new WriteMultipleCoilsRequest(1, startAddress, new DiscreteCollection(val, numberOfPoints)), 1, _testDataStore.CoilDiscretes);
			AssertModbusMessagePropertiesAreEqual(expectedResponse, response);
			Assert.AreEqual(new bool[] { val, val, val, val, val, val, val, val, val, val }, CollectionUtil.Slice<bool>(_testDataStore.CoilDiscretes, startAddress - 1, numberOfPoints));
		}
	}
}
