using System;
using System.Net.Sockets;
using Unme.Common;

namespace Modbus.IntegrationTests
{
	public static class TcpListenerUtility
	{
		public static Scope ScopedStart(this TcpListener listener)
		{
			if (listener == null)
				throw new ArgumentNullException("listener");

			var result = Scope.Create(listener.Stop);
			listener.Start();

			return result;
		}
	}
}
