using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modbus.Utility
{
	/// <summary>
	/// Various utility extension methods
	/// </summary>
	public static class UtilityExtensions
	{
		/// <summary>
		/// Creates a sequence containing this element and any optional additionalItems
		/// </summary>
		public static IEnumerable<T> ToSequence<T>(this T source, params T[] additionalItems)
		{
			if (additionalItems == null)
				throw new ArgumentNullException("additionalItems");

			yield return source;

			foreach (var item in additionalItems)
				yield return item;
		}

		/// <summary>
		///  Indicates whether the specified System.String object is null or an System.String.Empty string.
		/// </summary>
		/// <param name="source">A System.String reference.</param>
		/// <returns> true if the value parameter is null or an empty string (""); otherwise, false.</returns>
		public static bool IsNullOrEmpty(this string source)
		{
			return String.IsNullOrEmpty(source);
		}
	}
}
