using System;
using System.Collections.ObjectModel;
using Modbus.Util;

namespace Modbus.Data
{
	/// <summary>
	/// Object simulation of device memory map.
	/// </summary>
	public class DataStore
	{
		private DiscreteCollection _coilDiscretes = new DiscreteCollection();
		private DiscreteCollection _inputDiscetes = new DiscreteCollection();
		private RegisterCollection _holdingRegisters = new RegisterCollection();
		private RegisterCollection _inputRegisters = new RegisterCollection();

		/// <summary>
		/// Gets or sets the coil discretes.
		/// </summary>
		public DiscreteCollection CoilDiscretes
		{
			get { return _coilDiscretes; }
			set { _coilDiscretes = value; }
		}

		/// <summary>
		/// Gets or sets the input discretes.
		/// </summary>
		public DiscreteCollection InputDiscretes
		{
			get { return _inputDiscetes; }
			set { _inputDiscetes = value; }
		}

		/// <summary>
		/// Gets or sets the holding registers.
		/// </summary>
		public RegisterCollection HoldingRegisters
		{
			get { return _holdingRegisters; }
			set { _holdingRegisters = value; }
		}

		/// <summary>
		/// Gets or sets the input registers.
		/// </summary>
		public RegisterCollection InputRegisters
		{
			get { return _inputRegisters; }
			set { _inputRegisters = value; }
		}

		/// <summary>
		/// Retrieves subset of data from collection.
		/// </summary>
		/// <typeparam name="T">The collection type.</typeparam>
		/// <typeparam name="U">The type of elements in the collection.</typeparam>
		public static T ReadData<T, U>(T dataSource, ushort startAddress, ushort count) where T : Collection<U>, IModbusMessageDataCollection, new()
		{
			int startIndex = startAddress + 1;

			if (startIndex < 0 || startIndex >= dataSource.Count)
				throw new ArgumentOutOfRangeException("Start address was out of range. Must be non-negative and <= the size of the collection.");

			if (dataSource.Count < startIndex + count)
				throw new ArgumentOutOfRangeException("Read is outside valid range.");

			U[] dataToRetrieve = CollectionUtil.Slice(dataSource, startIndex, count);
			T result = new T();

			for (int i = 0; i < count; i++)
				result.Add(dataToRetrieve[i]);

			return result;
		}

		/// <summary>
		/// Write data to data store.
		/// </summary>
		/// <typeparam name="T">The collection type.</typeparam>
		/// <typeparam name="U">The type of elements in the collection.</typeparam>
		public static void WriteData<T, U>(T items, T destination, ushort startAddress) where T : Collection<U>, IModbusMessageDataCollection, new()
		{
			int startIndex = startAddress + 1;

			if (startIndex < 0 || startIndex >= destination.Count)
				throw new ArgumentOutOfRangeException("Start address was out of range. Must be non-negative and <= the size of the collection.");
			
			if (destination.Count < startIndex + items.Count)
				throw new ArgumentOutOfRangeException("Items collection is too large to write at specified start index.");

			CollectionUtil.Update(items, destination, startIndex);
		}
	}
}
