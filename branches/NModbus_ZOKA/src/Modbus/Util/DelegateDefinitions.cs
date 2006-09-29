using System;
using System.Collections.Generic;
using System.Text;

namespace Modbus.Util
{
	public delegate T Func<T>();
	public delegate T Func<T, A0>(A0 arg0);

	public delegate void Proc();
	public delegate void Proc<A0>(A0 arg0);
}
