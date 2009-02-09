using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Unme.Common;

namespace Modbus.IO
{
	/// <summary>
	/// Concrete Implementor - http://en.wikipedia.org/wiki/Bridge_Pattern
	/// </summary>
	internal class UdpClientAdapter : IStreamResource
	{
		private const int InfiniteTimeoutValue = 0;
		private UdpClient _udpClient;
		private List<byte> _readBuffer;

		public UdpClientAdapter(UdpClient udpClient)
		{			
			Debug.Assert(udpClient != null, "Argument udpClient cannot be null.");

			_udpClient = udpClient;
		}

		public int InfiniteTimeout
		{
			get { return InfiniteTimeoutValue; }
		}

		public int ReadTimeout
		{
			get { return _udpClient.Client.ReceiveTimeout; }
			set { _udpClient.Client.ReceiveTimeout = value; }
		}

		public int WriteTimeout
		{
			get { return _udpClient.Client.SendTimeout; }
			set { _udpClient.Client.SendTimeout = value; }
		}

		public void DiscardInBuffer()
		{
			// no-op
		}

		public int Read(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
				throw new ArgumentNullException("buffer");
			if (offset < 0)
				throw new ArgumentOutOfRangeException("offset", "Argument offset must be greater than or equal to 0.");
			if (offset > buffer.Length)
				throw new ArgumentOutOfRangeException("offset", "Argument offset cannot be greater than the length of buffer.");
			if (count < 0)
				throw new ArgumentOutOfRangeException("count", "Argument count must be greater than or equal to 0.");
			if (count > buffer.Length - offset)
				throw new ArgumentOutOfRangeException("count", "Argument count cannot be greater than the length of buffer minus offset.");

			IPEndPoint remoteIpEndPoint = null;
			if (_readBuffer == null || _readBuffer.Count() == 0)
				_readBuffer = Read(ref remoteIpEndPoint).ToList();

			if (_readBuffer.Count() < count)
				throw new IOException("Not enough bytes in the datagram.");

			_readBuffer.CopyTo(0, buffer, offset, count);
			_readBuffer.RemoveRange(0, count);

			return count;
		}

		public void Write(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
				throw new ArgumentNullException("buffer");
			if (offset < 0)
				throw new ArgumentOutOfRangeException("offset", "Argument offset must be greater than or equal to 0.");
			if (offset > buffer.Length)
				throw new ArgumentOutOfRangeException("offset", "Argument offset cannot be greater than the length of buffer.");

			_udpClient.Send(buffer.Skip(offset).ToArray(), count);
		}

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

		/// <summary>
		/// This method facilitates unit testing.
		/// </summary>
		internal virtual byte[] Read(ref IPEndPoint remoteIpEndPoint)
		{
			return _udpClient.Receive(ref remoteIpEndPoint);
		}

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                DisposableUtility.Dispose(ref _udpClient);
        }
	}
}
