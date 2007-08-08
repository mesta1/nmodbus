using System.Net.Sockets;
using System;

namespace Modbus.IO
{
	internal delegate int StreamReadWriteDelegate(byte[] buffer, int offset, int count);

	internal class TcpStreamAdapter
	{
		private readonly NetworkStream _networkStream;

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

		public virtual IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			return _networkStream.BeginWrite(buffer, offset, size, callback, state);
		}

		public virtual void EndWrite(IAsyncResult asyncResult)
		{
			_networkStream.EndWrite(asyncResult);
		}

		public virtual int Read(byte[] buffer, int offset, int size)
		{
			return _networkStream.Read(buffer, offset, size);
		}

		public virtual IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			return _networkStream.BeginRead(buffer, offset, size, callback, state);
		}

		public virtual int EndRead(IAsyncResult asyncResult)
		{
			return _networkStream.EndRead(asyncResult);
		}
	}
}
