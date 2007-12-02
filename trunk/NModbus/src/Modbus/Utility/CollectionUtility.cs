using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Modbus.Utility
{
	/// <summary>
	/// Provides methods for manipulating collections.
	/// </summary>
	public static class CollectionUtility
	{		
		/// <summary>
		/// Updates subset of values in a collection.
		/// </summary>
		public static void Update<T>(IList<T> items, IList<T> destination, int startIndex)
		{
			if (startIndex < 0 || destination.Count < startIndex + items.Count)
				throw new ArgumentOutOfRangeException("Index was out of range. Must be non-negative and less than the size of the collection.");

			for (int i = 0; i < items.Count; i++)
				destination[i + startIndex] = items[i];
		}

		/// <summary>
		/// Creates a collection initialized to a default value.
		/// </summary>
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