namespace Modbus.Utility
{
	/// <summary>
	/// Generic delegate definition.
	/// </summary>
	public delegate T Func<T>();

	/// <summary>
	/// Generic delegate definition.
	/// </summary>
	public delegate T Func<T, A0>(A0 arg0);

	/// <summary>
	/// Generic delegate definition.
	/// </summary>
	public delegate void Proc();

	/// <summary>
	/// Generic delegate definition.
	/// </summary>
	public delegate void Proc<A0>(A0 arg0);
}