using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Modbus.Data
{
	struct DiscreteMixin<T> where T : ICollection
	{
		public interface IMixin : IModbusMessageDataCollection
		{
		}

		public byte[] GetNetworkBytes(T that)
		{
			bool[] bits = new bool[that.Count];
			that.CopyTo(bits, 0);

			BitArray bitArray = new BitArray(bits);

			byte[] bytes = new byte[that.Count / 8 + (that.Count % 8 > 0 ? 1 : 0)];
			bitArray.CopyTo(bytes, 0);

			return bytes;
		}
	}
}
