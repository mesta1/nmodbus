using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modbus.Utility
{
	/// <summary>
	/// Provides a set of static (Shared in Visual Basic) methods for querying or modifying objects that implement IEnumerable<T>.
	/// </summary>
	public static class Enumerable
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

		public static void ForEachWithIndex<T>(this IEnumerable<T> source, Action<T, int> action)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			if (action == null)
				throw new ArgumentNullException("action");

			WithIndex(source).ForEach(pair => action(pair.Value, pair.Index));
		}
		
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
	}

	public struct IndexValuePair<T>
	{
		public IndexValuePair(int index, T value)
		{
			m_index = index;
			m_value = value;
		}

		public int Index
		{
			get { return m_index; }
		}
		public T Value
		{
			get { return m_value; }
		}

		readonly int m_index;
		readonly T m_value;
	}
}
