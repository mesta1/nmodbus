using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using MbUnit.Framework;
using Modbus.IO;
using Rhino.Mocks;
using Unme.MbUnit.Framework.Extensions;

namespace Modbus.UnitTests.IO
{
	[TestFixture]
	public class UdpClientAdapterFixture
	{
		[Test]
		public void Read_ArgumentValidation()
		{
			var adapter = new UdpClientAdapter(new UdpClient());

			// buffer
			AssertUtility.Throws<ArgumentNullException>(() => adapter.Read(null, 1, 1));
			
			// offset
			AssertUtility.Throws<ArgumentOutOfRangeException>(() => adapter.Read(new byte[2], -1, 2));
			AssertUtility.Throws<ArgumentOutOfRangeException>(() => adapter.Read(new byte[2], 3, 3));

			AssertUtility.Throws<ArgumentOutOfRangeException>(() => adapter.Read(new byte[2], 0, -1));
			AssertUtility.Throws<ArgumentOutOfRangeException>(() => adapter.Read(new byte[2], 1, 2));
		}

		[Test, ExpectedException(typeof(IOException))]
		public void Read_NotEnoughInBuffer()
		{
			var mocks = new MockRepository();
			var adapter = mocks.PartialMock<UdpClientAdapter>(new UdpClient());

			IPEndPoint endPoint = null;
			Expect.Call(adapter.Read(ref endPoint))
				.Return(new byte[] { 1, 2, 3, 4, 5 });

			mocks.ReplayAll();

			var buffer = new byte[5];

			// read first part of message
			Assert.AreEqual(4, adapter.Read(buffer, 0, 4));

			// read remainder... not enough in buffer
			Assert.AreEqual(2, adapter.Read(buffer, 3, 2));
		}

		[Test]
		public void Read_SingleMessageInTwoParts()
		{
			var mocks = new MockRepository();
			var adapter = mocks.PartialMock<UdpClientAdapter>(new UdpClient());

			IPEndPoint endPoint = null;
			Expect.Call(adapter.Read(ref endPoint))
				.Return(new byte[] { 1, 2, 3, 4, 5 });

			mocks.ReplayAll();

			var buffer = new byte[5];

			// read first part of message
			Assert.AreEqual(3, adapter.Read(buffer, 0, 3));
			
			// read remainder
			Assert.AreEqual(2, adapter.Read(buffer, 3, 2));
			
			Assert.AreEqual(new byte[] { 1, 2, 3, 4, 5 }, buffer);

			mocks.VerifyAll();
		}

		[Test]
		public void Read_TwoMessages()
		{
			var mocks = new MockRepository();
			var adapter = mocks.PartialMock<UdpClientAdapter>(new UdpClient());

			IPEndPoint endPoint = null;

			// first datagram
			Expect.Call(adapter.Read(ref endPoint))
				.Return(new byte[] { 1 });

			// second datagram
			Expect.Call(adapter.Read(ref endPoint))
				.Return(new byte[] { 2, 3, 4 });

			mocks.ReplayAll();

			// read first datagram
			var buffer = new byte[1];
			Assert.AreEqual(1, adapter.Read(buffer, 0, 1));

			// read second datagram
			buffer = new byte[3];
			Assert.AreEqual(3, adapter.Read(buffer, 0, 3));

			Assert.AreEqual(new byte[] { 2, 3, 4 }, buffer);

			mocks.VerifyAll();
		}

		[Test]
		public void Write_ArgumentValidation()
		{
			var adapter = new UdpClientAdapter(new UdpClient());

			// buffer 
			AssertUtility.Throws<ArgumentNullException>(() => adapter.Write(null, 1, 1));

			// offset
			AssertUtility.Throws<ArgumentOutOfRangeException>(() => adapter.Write(new byte[2], -1, 2));
			AssertUtility.Throws<ArgumentOutOfRangeException>(() => adapter.Write(new byte[2], 3, 3));
		}
	}
}
