using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections;

namespace Modbus.Util
{
	/// <summary>
	/// Provides methods for manipulating collections.
	/// </summary>
	public static class CollectionUtil
	{
		/// <summary>
		/// Returns a subset array of type T.
		/// </summary>
		public static T[] Slice<T>(T[] collection, int startIndex, int size)
		{
			if (collection == null)
				throw new ArgumentNullException("collection");

			T[] subset = new T[size];
			Array.Copy(collection, startIndex, subset, 0, size);

			return subset;
		}

		/// <summary>
		/// Returns a subset array of type T.
		/// </summary>
	    public static T[] Slice<T>(ICollection<T> collection, int startIndex, int size)
		{
			if (collection == null)
				throw new ArgumentNullException("collection");

			T[] collectionArray = new T[collection.Count];
			collection.CopyTo(collectionArray, 0);

			return Slice<T>(collectionArray, startIndex, size);
		}

		/// <summary>
		/// Returns an ICollection&lt;&gt;'s elements as an array.
		/// </summary>
		public static T[] ToArray<T>(ICollection<T> collection)
		{
			if (collection == null)
				throw new ArgumentNullException("collection");

			return Slice<T>(collection, 0, collection.Count);
		}

		/// <summary>
		/// Returns a boolean array representing the values stored in the BitArray.
		/// </summary>
		public static bool[] ToBoolArray(BitArray bitArray)	
		{
			if (bitArray == null)
				throw new ArgumentNullException("bitArray");

			bool[] bits = new bool[bitArray.Count];
			bitArray.CopyTo(bits, 0);

			return bits;
		}

		public static T[] Combine<T>(T[] array1, T[] array2)
		{
			if (array1 == null)
				throw new ArgumentNullException("array1");

			if (array2 == null)
				throw new ArgumentNullException("array2");

			T[] result = new T[array1.Length + array2.Length];

			Array.Copy(array1, result, array1.Length);
			Array.Copy(array2, 0, result, array1.Length, array2.Length);

			return result;
		}

		public static void Update<T>(IList<T> items, IList<T> destination, int startIndex)
		{
			if (startIndex < 0 || destination.Count < startIndex + items.Count)
				throw new ArgumentOutOfRangeException("Index was out of range. Must be non-negative and less than the size of the collection.");

			for (int i = 0; i < items.Count; i++)
				destination[i + startIndex] = items[i];
		}

		public static T CreateDefaultCollection<T, V>(V defaultValue, int size) where T : ICollection<V>, new()
		{
			if (size < 0)
				throw new ArgumentOutOfRangeException("Collection size cannot be less than 0.");

			T col = new T();

			for (int i = 0; i < size; i++)
				col.Add(defaultValue);

			return col;
		}
	}
}
