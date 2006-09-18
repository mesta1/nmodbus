using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Modbus.Util;
using System.Collections.ObjectModel;

namespace Modbus.Data
{
	public class DataStore
	{
		private DiscreteCollection _coilDiscretes = new DiscreteCollection();
		private DiscreteCollection _inputDiscetes = new DiscreteCollection();
		private RegisterCollection _holdingRegisters = new RegisterCollection();
		private RegisterCollection _inputRegisters = new RegisterCollection();

		public DataStore()
		{
		}

		public DiscreteCollection CoilDiscretes
		{
			get { return _coilDiscretes; }
		}

		public DiscreteCollection InputDiscretes
		{
			get { return _inputDiscetes; }
		}

		public RegisterCollection HoldingRegisters
		{
			get { return _holdingRegisters; }
		}

		public RegisterCollection InputRegisters
		{
			get { return _inputRegisters; }
		}

		/// <summary>
		/// Retrieves subset of data from collection.
		/// </summary>
		/// <typeparam name="T">The collection type.</typeparam>
		/// <typeparam name="U">The type of elements in the collection.</typeparam>
		public static T ReadData<T, U>(T dataSource, ushort startAddress, ushort count) where T : Collection<U>, IModbusMessageDataCollection, new()
		{
			int startIndex = startAddress - 1;

			if (startIndex >= dataSource.Count)
				throw new ArgumentOutOfRangeException("Start address was out of range. Must be non-negative and <= the size of the collection.");

			if (dataSource.Count < startIndex + count)
				throw new ArgumentOutOfRangeException("Read is outside valid range.");

			U[] dataToRetrieve = CollectionUtil.Slice(dataSource, startIndex, count);
			T result = new T();

			for (int i = 0; i < count; i++)
				result.Add(dataToRetrieve[i]);

			return result;
		}

		public static void WriteData<T, U>(T items, T destination, ushort startAddress) where T : Collection<U>, IModbusMessageDataCollection, new()
		{
			int startIndex = startAddress - 1;

			if (startIndex < 0 || startIndex >= destination.Count)
				throw new ArgumentOutOfRangeException("Start address was out of range. Must be non-negative and <= the size of the collection.");
			
			if (destination.Count < startIndex + items.Count)
				throw new ArgumentOutOfRangeException("Items collection is too large to write at specified start index.");

			CollectionUtil.Update(items, destination, startIndex);
		}
	}
}
