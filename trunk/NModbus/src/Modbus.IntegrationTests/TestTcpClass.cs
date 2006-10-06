using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Modbus.IntegrationTests
{
	[TestFixture]
	public class TestTcpClass
	{
		public TcpListener Server;
		public TcpClient Client;
		public Thread ServerThread;
		public IPAddress ip = new IPAddress(new byte[] { 127, 0, 0, 1 });

		[Test]
		public void TestTcp()
		{
			// start the server
			Server = new TcpListener(ip, 502);
			ServerThread = new Thread(Listen);
			ServerThread.Start();

			Console.WriteLine("create client");
			TcpClient client = new TcpClient(ip.ToString(), 502);
			Console.WriteLine("writing byte from client");
			client.GetStream().WriteByte(45);

		}

		public void Listen()
		{
			Server.Start();

			
				// waiting for a client...
				Client = Server.AcceptTcpClient();
				
				// got a client, read the bytes
				NetworkStream stream = Client.GetStream();
				int b = stream.ReadByte();
				Console.WriteLine("read {0}", b);
			
				Console.WriteLine("close connection w/ client");
				stream.Close();
				Client.Close();
		}

		[TestFixtureTearDownAttribute]
		public void TearDown()
		{
			Console.WriteLine("tear it down");
			Server.Stop();
			ServerThread.Abort();
		}
	}
}
