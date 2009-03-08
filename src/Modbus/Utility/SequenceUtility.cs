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

            return ToList(sequence, delegate(T item) { return item; });
        }

        /// <summary>
        /// Builds an IList&lt;T&gt; from an IEnumerable&lt;T&gt; sequence.
        /// </summary>
        public static IList<V> ToList<T, V>(IEnumerable<T> sequence, Func<T, V> projection)
        {
            if (sequence == null)
                throw new ArgumentNullException("sequence");

            List<V> list = new List<V>();
            foreach (T item in sequence)
                list.Add(projection(item));

            return list;
        }
    }
}
