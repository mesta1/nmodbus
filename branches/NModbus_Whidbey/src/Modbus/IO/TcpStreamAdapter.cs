using System;
using System.Net.Sockets;

namespace Modbus.IO
{
	internal delegate int StreamReadWriteDelegate(byte[] buffer, int offset, int count);

	internal class TcpStreamAdapter
	{
		private readonly NetworkStream _networkStream;

		public TcpStreamAdapter()
		{
		}

		public TcpStreamAdapter(NetworkStream networkStream)
		{
			_networkStream = networkStream;
		}		

		public virtual void Close()
		{
			_networkStream.Close();
		}

		public virtual void Write(byte[] buffer, int offset, int size)
		{
			_networkStream.Write(buffer, offset, size);
		}

		public virtual int Read(byte[] buffer, int offset, int size)
		{
			return _networkStream.Read(buffer, offset, size);
		}
	}
}
