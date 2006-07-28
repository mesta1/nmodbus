using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Modbus.Data
{
	struct RegisterMixin<T> where T : ICollection
	{
		public interface IMixin : IModbusMessageDataCollection
		{
		}

		public byte[] GetBytes(T that)
		{
			List<byte> bytes = new List<byte>();

			foreach (ushort register in that)
				bytes.AddRange(BitConverter.GetBytes(register));

			return bytes.ToArray();
		}
	}
}
