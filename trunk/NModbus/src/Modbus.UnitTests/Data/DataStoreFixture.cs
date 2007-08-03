using System;
using Modbus.Data;
using Modbus.Util;
using NUnit.Framework;

namespace Modbus.UnitTests.Data
{
	[TestFixture]
	public class DataStoreFixture
	{
		[Test]
		public void ReadData()
		{
			RegisterCollection slaveCol = new RegisterCollection(0, 1, 2, 3, 4, 5, 6);
			RegisterCollection result = DataStore.ReadData<RegisterCollection, ushort>(slaveCol, 1, 3);
			Assert.AreEqual(new ushort[] { 2, 3, 4 }, CollectionUtil.ToArray<ushort>(result));
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ReadDataStartAddressTooLarge()
		{
			DataStore.ReadData<DiscreteCollection, bool>(new DiscreteCollection(), 3, 2);
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ReadDataCountTooLarge()
		{ 
			DataStore.ReadData<DiscreteCollection, bool>(new DiscreteCollection(true, false, true, true), 1, 5);
		}

		[Test]
		public void ReadDataStartAddressZero()
		{
			DataStore.ReadData<DiscreteCollection, bool>(new DiscreteCollection(true, false, true, true, true, true), 0, 5);
		}

		[Test]
		public void WriteDataSingle()
		{
			DiscreteCollection destination = new DiscreteCollection(true, true);
			DiscreteCollection newValues = new DiscreteCollection(false);
			DataStore.WriteData<DiscreteCollection, bool>(newValues, destination, 0);
			Assert.AreEqual(false, destination[1]);
		}

		[Test]
		public void WriteDataMultiple()
		{
			DiscreteCollection destination = new DiscreteCollection(true, false, false, false, false, false, true);
			DiscreteCollection newValues = new DiscreteCollection(true, true, true, true);
			DataStore.WriteData<DiscreteCollection, bool>(newValues, destination, 0);
			Assert.AreEqual(new bool[] { true, true, true, true, true, false, true }, CollectionUtil.ToArray(destination));
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void WriteDataTooLarge()
		{
			DiscreteCollection slaveCol = new DiscreteCollection(true);
			DiscreteCollection newValues = new DiscreteCollection(false, false);
			DataStore.WriteData<DiscreteCollection, bool>(newValues, slaveCol, 1);
		}

		[Test]
		public void WriteDataStartAddressZero()
		{
			DataStore.WriteData<DiscreteCollection, bool>(new DiscreteCollection(false), new DiscreteCollection(true, true), 0);
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void WriteDataStartAddressTooLarge()
		{
			DataStore.WriteData<DiscreteCollection, bool>(new DiscreteCollection(true), new DiscreteCollection(true), 2);
		}

		/// <summary>
		/// http://modbus.org/docs/Modbus_Application_Protocol_V1_1b.pdf
		/// In the PDU Coils are addressed starting at zero. Therefore coils numbered 1-16 are addressed as 0-15.
		/// So reading address 0 should get you 
		/// </summary>
		[Test]
		public void TestReadMapping()
		{
			DataStore dataStore = DataStoreFactory.CreateDefaultDataStore();
			dataStore.HoldingRegisters.Insert(1, 45);
			dataStore.HoldingRegisters.Insert(2, 42);

			Assert.AreEqual(45, DataStore.ReadData<RegisterCollection, ushort>(dataStore.HoldingRegisters, 0, 1)[0]);
			Assert.AreEqual(42, DataStore.ReadData<RegisterCollection, ushort>(dataStore.HoldingRegisters, 1, 1)[0]);
		}
	}
}
