using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections;

namespace Modbus.Util
{
	public static class CollectionUtil
	{
		/// <summary>
		/// Returns a subset array of type T.
		/// </summary>
		public static T[] Slice<T>(T[] collection, int startIndex, int size)
		{
			T[] subset = new T[size];
			Array.Copy(collection, startIndex, subset, 0, size);

			return subset;
		}

		/// <summary>
		/// Returns a subset array of type T.
		/// </summary>
	    public static T[] Slice<T>(ICollection<T> collection, int startIndex, int size)
		{
			T[] collectionArray = new T[collection.Count];
			collection.CopyTo(collectionArray, 0);

			return Slice<T>(collectionArray, startIndex, size);
		}

		/// <summary>
		/// Returns an ICollection<>'s elements as an array.
		/// </summary>
		public static T[] ToArray<T>(ICollection<T> collection)
		{
			return Slice<T>(collection, 0, collection.Count);
		}

		/// <summary>
		/// Returns a boolean array representing the values stored in the BitArray.
		/// </summary>
		public static bool[] ToBoolArray(BitArray bitArray)	
		{
			if (bitArray == null)
				throw new ArgumentNullException("bitArray", "Argument cannot be null.");

			bool[] bits = new bool[bitArray.Count];
			bitArray.CopyTo(bits, 0);
			return bits;
		}
	}
}
