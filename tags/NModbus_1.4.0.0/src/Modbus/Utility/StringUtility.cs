using System;
using System.Collections.Generic;
using Modbus.Utility;

namespace Modbus.Utility
{
	/// <summary>
	/// String utility methods.
	/// </summary>
	public static class StringUtility
	{
		/// <summary>
		/// Concatenates a specified separator String between each element of a specified collection, 
		/// yielding a single concatenated string. 
		/// </summary>
		public static string Join<T>(string separator, ICollection<T> collection)
		{
			return Join(separator, collection, DefaultConversion);
		}

		/// <summary>
		/// Concatenates a specified separator String between each converted element of a specified collection, 
		/// yielding a single concatenated string. 
		/// </summary>
		public static string Join<T>(string separator, ICollection<T> collection, Converter<T, string> conversion)
		{
			if (collection == null)
				throw new ArgumentNullException("collection");

			if (conversion == null)
				throw new ArgumentNullException("conversion");

			return Join(separator, CollectionUtility.ToArray(collection), conversion);			
		}

		/// <summary>
		/// Concatenates a specified separator String between each element of a specified array, 
		/// yielding a single concatenated string. 
		/// </summary>
		public static string Join<T>(string separator, T[] collection)
		{
			return Join(separator, collection, DefaultConversion);
		}

		/// <summary>
		/// Concatenates a specified separator String between each converted element of a specified array, 
		/// yielding a single concatenated string. 
		/// </summary>
		public static string Join<T>(string separator, T[] collection, Converter<T, string> conversion)
		{
			if (collection == null)
				throw new ArgumentNullException("collection");

			if (conversion == null)
				throw new ArgumentNullException("conversion");

			return String.Join(separator, Array.ConvertAll(collection, conversion));
		}

		private static string DefaultConversion<T>(T t)
		{
			return t.ToString();
		}
	}
}