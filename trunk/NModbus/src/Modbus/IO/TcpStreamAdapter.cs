using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace Modbus.IO
{
	internal delegate int StreamReadWriteDelegate(byte[] buffer, int offset, int count);

	internal class TcpStreamAdapter
	{
		private NetworkStream _networkStream;

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

		public virtual void Write(byte[] buffer, int offset, int count)
		{
			_networkStream.Write(buffer, offset, count);
		}

		public virtual int Read(byte[] buffer, int offset, int count)
		{
			return _networkStream.Read(buffer, offset, count);
		}
	}
}
