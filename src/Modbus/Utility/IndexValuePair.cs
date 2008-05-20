namespace Modbus.Utility
{
	/// <summary>
	/// Immutable index value pair
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public struct IndexValuePair<T>
	{
		private readonly int _index;
		private readonly T _value;

		/// <summary>
		/// Initializes a new instance of the <see cref="IndexValuePair&lt;T&gt;"/> struct.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		public IndexValuePair(int index, T value)
		{
			_index = index;
			_value = value;
		}

		/// <summary>
		/// Gets the index.
		/// </summary>
		/// <value>The index.</value>
		public int Index
		{
			get { return _index; }
		}

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <value>The value.</value>
		public T Value
		{
			get { return _value; }
		}	
	}
}
