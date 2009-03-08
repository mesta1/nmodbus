
namespace Modbus.Utility
{
    /// <summary>
    /// Generic delegate definition.
    /// </summary>
    public delegate T Func<T>();

    /// <summary>
    /// Generic delegate definition.
    /// </summary>
    public delegate T Func<A0, T>(A0 arg0);

    /// <summary>
    /// Generic delegate definition.
    /// </summary>
    public delegate T Func<A0, A1, T>(A0 arg0, A1 arg1);

    /// <summary>
    /// Generic delegate definition.
    /// </summary>
    public delegate void Action();

    /// <summary>
    /// Generic delegate definition.
    /// </summary>
    public delegate void Action<A0>(A0 arg0);
}
