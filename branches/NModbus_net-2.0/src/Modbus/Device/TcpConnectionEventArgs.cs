using System;

namespace Modbus.Device
{
	internal class TcpConnectionEventArgs : EventArgs
	{
		public TcpConnectionEventArgs(string endPoint)
		{
			if (endPoint == null)
				throw new ArgumentNullException("endPoint");
			if (String.IsNullOrEmpty(endPoint))
				throw new ArgumentException("Argument endPoint cannot be empty.");

			EndPoint = endPoint;
		}

		public string EndPoint { get; set; }
	}
}
