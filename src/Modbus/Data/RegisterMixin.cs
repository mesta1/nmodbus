using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Modbus.Data
{
	struct RegisterMixin<T> where T : ICollection
	{
		public interface IMixin : IModbusMessageDataCollection
		{
		}

		public byte[] GetNetworkBytes(T that)
		{
			List<byte> bytes = new List<byte>();

			foreach (ushort register in that)
				bytes.AddRange(BitConverter.GetBytes((ushort) IPAddress.HostToNetworkOrder((short) register)));

			return bytes.ToArray();
		}
	}
}
