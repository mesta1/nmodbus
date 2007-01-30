using System;
using System.Collections.Generic;
using System.Text;

namespace Modbus.Util
{
	public static class StringUtil
	{
		public static string Join<T>(string separator, ICollection<T> collection)
		{
			return Join(separator, collection, DefaultConversion);
		}
		
		public static string Join<T>(string separator, ICollection<T> collection, Converter<T, string> conversion)
		{
			if (collection == null)
				throw new ArgumentNullException("collection");

			if (conversion == null)
				throw new ArgumentNullException("conversion");

			return Join(separator, CollectionUtil.ToArray<T>(collection), conversion);			
		}

		public static string Join<T>(string separator, T[] collection)
		{
			return Join(separator, collection, DefaultConversion);
		}

		public static string Join<T>(string separator, T[] collection, Converter<T, string> conversion)
		{
			if (collection == null)
				throw new ArgumentNullException("collection");

			if (conversion == null)
				throw new ArgumentNullException("conversion");

			return String.Join(separator, Array.ConvertAll<T, string>(collection, conversion));
		}

		private static string DefaultConversion<T>(T t)
		{
			return t.ToString();
		}
	}
}
