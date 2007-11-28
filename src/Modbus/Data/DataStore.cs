using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Modbus.Utility;

namespace Modbus.Data
{
	/// <summary>
	/// Object simulation of device memory map.
	/// </summary>
	public class DataStore
	{
		private ModbusDataCollection<bool> _coilDiscretes = new ModbusDataCollection<bool>();
		private ModbusDataCollection<bool> _inputDiscretes = new ModbusDataCollection<bool>();
		private ModbusDataCollection<ushort> _holdingRegisters = new ModbusDataCollection<ushort>();
		private ModbusDataCollection<ushort> _inputRegisters = new ModbusDataCollection<ushort>();

		/// <summary>
		/// Gets or sets the coil discretes.
		/// </summary>
		public ModbusDataCollection<bool> CoilDiscretes
		{
			get { return _coilDiscretes; }
			set { _coilDiscretes = value; }
		}

		/// <summary>
		/// Gets or sets the input discretes.
		/// </summary>
		public ModbusDataCollection<bool> InputDiscretes
		{
			get { return _inputDiscretes; }
			set { _inputDiscretes = value; }
		}

		/// <summary>
		/// Gets or sets the holding registers.
		/// </summary>
		public ModbusDataCollection<ushort> HoldingRegisters
		{
			get { return _holdingRegisters; }
			set { _holdingRegisters = value; }
		}

		/// <summary>
		/// Gets or sets the input registers.
		/// </summary>
		public ModbusDataCollection<ushort> InputRegisters
		{
			get { return _inputRegisters; }
			set { _inputRegisters = value; }
		}

		/// <summary>
		/// Retrieves subset of data from collection.
		/// </summary>
		/// <typeparam name="T">The collection type.</typeparam>
		/// <typeparam name="U">The type of elements in the collection.</typeparam>
		internal static T ReadData<T, U>(ModbusDataCollection<U> dataSource, ushort startAddress, ushort count) where T : Collection<U>, new()
		{
			int startIndex = startAddress + 1;

			if (startIndex < 0 || startIndex >= dataSource.Count)
				throw new ArgumentOutOfRangeException("Start address was out of range. Must be non-negative and <= the size of the collection.");

			if (dataSource.Count < startIndex + count)
				throw new ArgumentOutOfRangeException("Read is outside valid range.");

			U[] dataToRetrieve = CollectionUtility.Slice(dataSource, startIndex, count);
			T result = new T();

			for (int i = 0; i < count; i++)
				result.Add(dataToRetrieve[i]);

			return result;
		}

		/// <summary>
		/// Write data to data store.
		/// </summary>
		/// <typeparam name="TData">The type of the data.</typeparam>
		internal static void WriteData<TData>(Collection<TData> items, ModbusDataCollection<TData> destination, ushort startAddress)
		{
			int startIndex = startAddress + 1;

			if (startIndex < 0 || startIndex >= destination.Count)
				throw new ArgumentOutOfRangeException("Start address was out of range. Must be non-negative and <= the size of the collection.");
			
			if (destination.Count < startIndex + items.Count)
				throw new ArgumentOutOfRangeException("Items collection is too large to write at specified start index.");

			CollectionUtility.Update(items, destination, startIndex);
		}
	}
}
