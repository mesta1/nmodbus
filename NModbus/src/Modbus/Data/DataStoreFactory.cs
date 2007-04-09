using System;
using System.Collections.Generic;
using System.Text;

namespace Modbus.Data
{
	/// <summary>
	/// Data story factory.
	/// </summary>
	public static class DataStoreFactory
	{
		const int DefaultSize = 3000;

		/// <summary>
		/// Factory method for test data store.
		/// </summary>
		public static DataStore CreateTestDataStore()
		{
			DataStore dataStore = new DataStore();

			for (int i = 0; i < DefaultSize; i++)
			{
				bool value = i % 2 > 0;
				dataStore.CoilDiscretes.Add(value);
				dataStore.InputDiscretes.Add(!value);
				dataStore.HoldingRegisters.Add((ushort) (i + 1));
				dataStore.InputRegisters.Add((ushort) ((i + 1) * 10));
			}			

			return dataStore;
		}

		/// <summary>
		/// Factory method for default data store.
		/// </summary>
		public static DataStore CreateDefaultDataStore()
		{
			DataStore dataStore = new DataStore();

			for (int i = 0; i < DefaultSize; i++)
			{
				dataStore.CoilDiscretes.Add(false);
				dataStore.InputDiscretes.Add(false);
				dataStore.HoldingRegisters.Add(0);
				dataStore.InputRegisters.Add(0);
			}

			return dataStore;
		}
	}
}
