using System;
using System.Collections.Generic;
using System.Text;

namespace Modbus.Data
{
	public static class DataStoreFactory
	{
		public static DataStore CreateTestDataStore()
		{
			DataStore dataStore = new DataStore();

			for (int i = 0; i < 400; i++)
			{
				bool value = i % 2 > 0;
				dataStore.CoilDiscretes.Add(value);
				dataStore.InputDiscretes.Add(!value);
				dataStore.HoldingRegisters.Add((ushort) (i + 1));
				dataStore.InputRegisters.Add((ushort) ((i + 1) * 10));
			}			

			return dataStore;
		}
	}
}
