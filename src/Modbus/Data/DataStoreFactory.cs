using System;

namespace Modbus.Data
{
	/// <summary>
	/// Data story factory.
	/// </summary>
	public static class DataStoreFactory
	{		
		/// <summary>
		/// Factory method for default data store - register values set to 0 and discrete values set to false.
		/// </summary>
		public static DataStore CreateDefaultDataStore()
		{
			return CreateDefaultDataStore(UInt16.MaxValue, UInt16.MaxValue, UInt16.MaxValue, UInt16.MaxValue);
		}

		/// <summary>
		/// Factory method for default data store - register values set to 0 and discrete values set to false.
		/// </summary>
		public static DataStore CreateDefaultDataStore(ushort coilsCount, ushort inputsCount, ushort holdingRegistersCount, ushort inputRegistersCount)
		{
			DataStore dataStore = new DataStore();

            for (int i = 0; i < coilsCount; i++)
                dataStore.CoilDiscretes.Add(false);
            for (int i = 0; i < inputsCount; i++)
                dataStore.InputDiscretes.Add(false);
            for (int i = 0; i < holdingRegistersCount; i++)
                dataStore.HoldingRegisters.Add((ushort) 0);
            for (int i = 0; i < inputRegistersCount; i++)
                dataStore.InputRegisters.Add((ushort) 0);

			return dataStore;
		}

		/// <summary>
		/// Factory method for test data store.
		/// </summary>
		public static DataStore CreateTestDataStore()
		{
			DataStore dataStore = new DataStore();

			for (int i = 1; i < 3000; i++)
			{
				bool value = i % 2 > 0;
				dataStore.CoilDiscretes.Add(value);
				dataStore.InputDiscretes.Add(!value);
				dataStore.HoldingRegisters.Add((ushort) (i));
				dataStore.InputRegisters.Add((ushort) ((i) * 10));
			}

			return dataStore;
		}
	}
}
