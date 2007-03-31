using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace Modbus.IO
{
	public class TcpTransportAdapter
	{
		private NetworkStream _networkStream;

		public TcpTransportAdapter()
		{
		}

		public TcpTransportAdapter(NetworkStream networkStream)
		{
			_networkStream = networkStream;
		}

		public NetworkStream NetworkStream
		{
			get 
			{ 
				return _networkStream; 
			}
			set
			{
				_networkStream = value;
			}
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
