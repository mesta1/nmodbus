using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Modbus.Utility;

namespace Modbus.Data
{
	/// <summary>
	/// Event args for read write actions performed on the DataStore.
	/// </summary>
	public class DataStoreEventArgs : EventArgs
	{
		private DataStoreEventArgs(ushort startAddress, ModbusDataType modbusDataType)
		{
			this.StartAddress = startAddress;
			this.ModbusDataType = modbusDataType;
		}

		/// <summary>
		/// Type of Modbus data (e.g. Holding register).
		/// </summary>
		public ModbusDataType ModbusDataType { get; private set; }

		/// <summary>
		/// Start address of data.
		/// </summary>
		public ushort StartAddress { get; private set; }

		/// <summary>
		/// Data that was read or written.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
		public DiscriminatedUnion<ReadOnlyCollection<bool>, ReadOnlyCollection<ushort>> Data { get; private set; }

		internal static DataStoreEventArgs CreateDataStoreEventArgs<T>(ushort startAddress, ModbusDataType modbusDataType, IList<T> data)
		{
			if (data == null)
				throw new ArgumentNullException("data");

			if (!(typeof(T) == typeof(bool) || typeof(T) == typeof(ushort)))
				throw new ArgumentException("Generic type T should be of type bool or ushort");

            DataStoreEventArgs eventArgs = new DataStoreEventArgs(startAddress, modbusDataType);

			if (typeof(T) == typeof(bool))
				eventArgs.Data = DiscriminatedUnion<ReadOnlyCollection<bool>, ReadOnlyCollection<ushort>>.CreateA(new ReadOnlyCollection<bool>((IList<bool>)data));
			else
				eventArgs.Data = DiscriminatedUnion<ReadOnlyCollection<bool>, ReadOnlyCollection<ushort>>.CreateB(new ReadOnlyCollection<ushort>((IList<ushort>)data));

			return eventArgs;
		}		
	}
}
