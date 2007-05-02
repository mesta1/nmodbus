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
			RegisterCollection slaveCol = new RegisterCollection(1, 2, 3, 4, 5, 6);
			RegisterCollection result = DataStore.ReadData<RegisterCollection, ushort>(slaveCol, 2, 3);
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

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ReadDataNegativeStartAddress()
		{
			DataStore.ReadData<DiscreteCollection, bool>(new DiscreteCollection(true, false, true, true), 0, 5);
		}

		[Test]
		public void WriteDataSingle()
		{
			DiscreteCollection destination = new DiscreteCollection(true);
			DiscreteCollection newValues = new DiscreteCollection(false);
			DataStore.WriteData<DiscreteCollection, bool>(newValues, destination, 1);
			Assert.AreEqual(false, destination[0]);
		}

		[Test]
		public void WriteDataMultiple()
		{
			DiscreteCollection destination = new DiscreteCollection(true, false, false, false, false, true);
			DiscreteCollection newValues = new DiscreteCollection(true, true, true, true);
			DataStore.WriteData<DiscreteCollection, bool>(newValues, destination, 1);
			Assert.AreEqual(new bool[] { true, true, true, true, false, true }, CollectionUtil.ToArray<bool>(destination));
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void WriteDataTooLarge()
		{
			DiscreteCollection slaveCol = new DiscreteCollection(true);
			DiscreteCollection newValues = new DiscreteCollection(false, false);
			DataStore.WriteData<DiscreteCollection, bool>(newValues, slaveCol, 1);
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void WriteDataStartAddressZero()
		{
			DataStore.WriteData<DiscreteCollection, bool>(new DiscreteCollection(true), new DiscreteCollection(true), 0);
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void WriteDataStartAddressTooLarge()
		{
			DataStore.WriteData<DiscreteCollection, bool>(new DiscreteCollection(true), new DiscreteCollection(true), 2);
		}
	}
}
