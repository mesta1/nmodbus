using System;
using System.Collections.Generic;

namespace Modbus.Util
{
	/// <summary>
	/// Utility class for IEnumerable<T>
	/// </summary>
	public class SequenceUtility
	{
		/// <summary>
		/// Builds an IList<T> from an IEnumerable<T> sequence.
		/// </summary>
		public static IList<T> ToList<T>(IEnumerable<T> sequence)
		{
			if (sequence == null)
				throw new ArgumentException("sequence");

			List<T> list = new List<T>();
			foreach (T item in sequence)
				list.Add(item);

			return list;
		}
	}
}
