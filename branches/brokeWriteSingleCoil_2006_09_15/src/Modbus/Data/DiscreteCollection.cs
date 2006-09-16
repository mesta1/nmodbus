using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Modbus.Util;


namespace Modbus.Data
{
	public class DiscreteCollection : Collection<bool>, IModbusMessageDataCollection
	{
		public DiscreteCollection ()
		{
		}

		public DiscreteCollection(params bool[] bits)
			: this((IList<bool>)bits)
		{
		}

		public DiscreteCollection(params byte[] bytes)
			: this((IList<bool>)CollectionUtil.ToBoolArray(new BitArray(bytes)))
		{
		}

		public DiscreteCollection(IList<bool> bits)
		    : base(bits.IsReadOnly ? new List<bool>(bits) : bits)
		{
		}

		/// <summary>
		/// Creates DiscreteCollection of specified size initialized to default value.
		/// </summary>
		public DiscreteCollection(bool defaultValue, int size)
		{
			for (int i = 0; i < size; i++)
				Add(defaultValue);
		}

		public byte[] NetworkBytes
		{
			get
			{
				bool[] bits = new bool[Count];
				CopyTo(bits, 0);

				BitArray bitArray = new BitArray(bits);

				byte[] bytes = new byte[Count / 8 + (Count % 8 > 0 ? 1 : 0)];
				bitArray.CopyTo(bytes, 0);

				return bytes;
			}
		}
	}
}
