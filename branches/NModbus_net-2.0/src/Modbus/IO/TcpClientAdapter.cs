using System;
using System.Diagnostics;
using System.Net.Sockets;

namespace Modbus.IO
{
	/// <summary>
	/// Concrete Implementor - http://en.wikipedia.org/wiki/Bridge_Pattern
	/// </summary>
	internal class TcpClientAdapter : IStreamResource
	{
		private const int InfiniteTimeoutValue = 0;
		private NetworkStream _networkStream;

		public TcpClientAdapter(TcpClient tcpClient)
		{
			Debug.Assert(tcpClient != null, "Argument tcpClient cannot be null.");

			_networkStream = tcpClient.GetStream();
		}

		public int InfiniteTimeout
		{
			get { return InfiniteTimeoutValue; }
		}

		public int ReadTimeout
		{
			get { return _networkStream.ReadTimeout; }
			set { _networkStream.ReadTimeout = value; }
		}

		public int WriteTimeout
		{
			get { return _networkStream.WriteTimeout; }
			set { _networkStream.WriteTimeout = value; }
		}		

		public void Write(byte[] buffer, int offset, int size)
		{
			_networkStream.Write(buffer, offset, size);
		}

		public int Read(byte[] buffer, int offset, int size)
		{
			return _networkStream.Read(buffer, offset, size);
		}

		public void DiscardInBuffer()
		{
			_networkStream.Flush();
		}

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_networkStream != null)
                {
                    _networkStream.Dispose();
                    _networkStream = null;
                }
            }
        }
    }
}
