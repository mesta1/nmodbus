using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Modbus.Utility
{
	/// <summary>
	/// Various utility extension methods
	/// </summary>
	public static class UtilityExtensions
	{
		/// <summary>
		/// Returns a new array with additional items concatenated onto the first.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="first">The first.</param>
		/// <param name="second">The second.</param>
		/// <param name="third">The third.</param>
		/// <param name="additionalItems">The additional items.</param>
		public static IEnumerable<T> Concat<T>(this IEnumerable<T> first, IEnumerable<T> second, IEnumerable<T> third, params IEnumerable<T>[] additionalItems)
		{
			if (second == null)
				throw new ArgumentNullException("firstItem");

			if (third == null)
				throw new ArgumentNullException("secondItem");

			if (additionalItems == null)
				throw new ArgumentNullException("additionalItems");

			first = first.Concat(second);
			first = first.Concat(third);
			additionalItems.ForEach((item) => first = first.Concat((IEnumerable<T>) item));

			return first;
		}

		/// <summary>
		/// Iterates the source applying the action.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <param name="action">The action.</param>
		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			if (action == null)
				throw new ArgumentNullException("action");

			foreach (T item in source)
				action(item);
		}

		/// <summary>
		/// Iterates the source applying the action with an index.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <param name="action">The action.</param>
		public static void ForEachWithIndex<T>(this IEnumerable<T> source, Action<T, int> action)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			if (action == null)
				throw new ArgumentNullException("action");

			WithIndex(source).ForEach(pair => action(pair.Value, pair.Index));
		}

		/// <summary>
		/// Iterates the source returning values with an index.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public static IEnumerable<IndexValuePair<T>> WithIndex<T>(this IEnumerable<T> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			int position = 0;
			foreach (T value in source)
				yield return new IndexValuePair<T>(position++, value);
		}

		/// <summary>
		/// Returns a slice of the given source.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="size">The size.</param>
		public static IEnumerable<T> Slice<T>(this IEnumerable<T> source, int startIndex, int size)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			int count = source.Count();
			if (startIndex < 0 || count < startIndex)
				throw new ArgumentOutOfRangeException("startIndex");

			if (size < 0 || startIndex + size > count)
				throw new ArgumentOutOfRangeException("count");

			return source.Skip(startIndex).Take(size);
		}

		/// <summary>
		/// Concatenates a specified separator String between each converted element of a specified collection, 
		/// yielding a single concatenated string. 
		/// </summary>
		public static string Join<T>(this IEnumerable<T> sequence, string separator)
		{
			return sequence.Join(separator, (item) => item.ToString());
		}

		/// <summary>
		/// Concatenates a specified separator String between each converted element of a specified collection, 
		/// yielding a single concatenated string. 
		/// </summary>
		public static string Join<T>(this IEnumerable<T> sequence, string separator, Converter<T, string> conversion)
		{
			if (separator == null)
				throw new ArgumentNullException("separator");

			if (conversion == null)
				throw new ArgumentNullException("conversion");

			return String.Join(separator, Array.ConvertAll(sequence.ToArray(), conversion));
		}

		/// <summary>
		/// Converts a sequence to a readonly collection of T.
		/// </summary>
		public static ReadOnlyCollection<T> ToReadOnly<T>(this IEnumerable<T> sequence)
		{
			if (sequence == null)
				throw new ArgumentNullException("sequence");

			return Array.AsReadOnly(sequence.ToArray());
		}

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

		/// <summary>
		/// Performs the specified action on the obj if it is not null.
		/// </summary>
		public static void IfNotNull<T>(this T obj, Action<T> action)
		{
			if (obj != null)
				action(obj);
		}

		/// <summary>
		/// Performs the specified func on the obj if it is not null. Returns the return value of the func or default of U if obj is null.
		/// </summary>
		public static U IfNotNull<T, U>(this T obj, Func<T, U> func)
		{
			return obj != null ? func(obj) : default(U);
		}
	}
}
