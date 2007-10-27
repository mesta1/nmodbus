using System;
using System.Collections.Generic;

namespace Modbus.Utility
{
	/// <summary>
	/// Utility class for IEnumerable&lt;T&gt;
	/// </summary>
	public class SequenceUtility
	{
		/// <summary>
		/// Builds an IList&lt;T&gt; from an IEnumerable&lt;T&gt; sequence.
		/// </summary>
		public static IList<T> ToList<T>(IEnumerable<T> sequence)
		{
			if (sequence == null)
				throw new ArgumentNullException("sequence");

			List<T> list = new List<T>();
			foreach (T item in sequence)
				list.Add(item);

			return list;
		}
	}
}