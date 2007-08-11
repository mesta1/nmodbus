using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Modbus.Utility;

namespace Modbus.Data
{
	/// <summary>
	/// Collection of discrete values.
	/// </summary>
	public class DiscreteCollection : Collection<bool>, IModbusMessageDataCollection
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DiscreteCollection"/> class.
		/// </summary>
		public DiscreteCollection ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DiscreteCollection"/> class.
		/// </summary>
		public DiscreteCollection(params bool[] bits)
			: this((IList<bool>)bits)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DiscreteCollection"/> class.
		/// </summary>
		public DiscreteCollection(params byte[] bytes)
			: this((IList<bool>)CollectionUtility.ToBoolArray(new BitArray(bytes)))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DiscreteCollection"/> class.
		/// </summary>
		public DiscreteCollection(IList<bool> bits)
		    : base(bits.IsReadOnly ? new List<bool>(bits) : bits)
		{
		}

		/// <summary>
		/// Gets the network bytes.
		/// </summary>
		public byte[] NetworkBytes
		{
			get
			{
				bool[] bits = new bool[Count];
				CopyTo(bits, 0);

				BitArray bitArray = new BitArray(bits);

				byte[] bytes = new byte[ByteCount];
				bitArray.CopyTo(bytes, 0);

				return bytes;
			}
		}

		/// <summary>
		/// Gets the byte count.
		/// </summary>
		public byte ByteCount
		{
			get
			{
				return (byte) ((Count + 7) / 8);
			}
		}
	}
}
