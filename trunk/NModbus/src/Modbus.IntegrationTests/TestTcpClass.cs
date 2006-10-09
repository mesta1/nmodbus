using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;

namespace Modbus.IntegrationTests
{
	[TestFixture]
	public class TestTcpClass
	{
		public TcpListener Slave;
		public IPAddress ip = new IPAddress(new byte[] { 127, 0, 0, 1 });

		[Test, Explicit]
		public void TestTcp()
		{
			// start the server
			Slave = new TcpListener(ip, 502);
			Thread serverThread = new Thread(Listen);
			serverThread.Start();

			Console.WriteLine("create client");
			TcpClient client = new TcpClient(ip.ToString(), 502);

			for (int i = 0; i < 5; i++)
			{
				Console.WriteLine("Client write {0}", i);
				client.GetStream().WriteByte((byte) i);
				Console.WriteLine("Client read {0}", client.GetStream().ReadByte());
			}

			client.GetStream().Close();
			client.Close();

			Slave.Stop();
			serverThread.Abort();
		}

		[Test, Explicit]
		public void TestTcpMultipleClients()
		{
			// start the server
			Slave = new TcpListener(ip, 502);
			Slave.Server.ReceiveTimeout = Slave.Server.SendTimeout = 200;
			Thread serverThread = new Thread(Listen);
			serverThread.Start();

			Console.WriteLine("create clients");
			TcpClient client1 = new TcpClient(ip.ToString(), 502);			
			TcpClient client2 = new TcpClient(ip.ToString(), 502);
			TcpClient client3 = new TcpClient(ip.ToString(), 502);

			// some comm
			for (int i = 0; i < 5; i++)
			{
				Console.WriteLine("Client1 write {0}", i);
				client1.GetStream().WriteByte((byte) i);
				Console.WriteLine("Client1 read {0}", client1.GetStream().ReadByte());
			}

			// this should cause a receive timeout in the server
			//Console.WriteLine("sleep for a couple seconds...");
			//Thread.Sleep(2000);

			Console.WriteLine("Client1 closing");
			client1.GetStream().Close();
			client1.Close();

			//Console.WriteLine("sleep for a couple seconds...");
			//Thread.Sleep(2000);

			
			for (int i = 0; i < 5; i++)
			{
				Console.WriteLine("Client2 write {0}", i);
				client2.GetStream().WriteByte((byte) i);
				Console.WriteLine("Client2 read {0}", client2.GetStream().ReadByte());
			}

			Console.WriteLine("Client2 closing");
			client2.GetStream().Close();
			client2.Close();

			Slave.Stop();
			serverThread.Abort();
			Thread.Sleep(100);
		}

		public void Listen()
		{
			Slave.Start();

			while (true)
			{
				NetworkStream stream = null;
				TcpClient client = null;

				try
				{
					Console.WriteLine("Server waiting for client...");
					client = Slave.AcceptTcpClient();
				
					// got a client, read the bytes
					Console.WriteLine("Server accepted client");
					stream = client.GetStream();

					int b;

					while ((b = stream.ReadByte()) != -1)
					{
						Console.WriteLine("Server read {0}", b);
						Console.WriteLine("Server write {0}", b);
						stream.WriteByte((byte) b);
					}
				}
				catch (IOException ioe)
				{
					Console.WriteLine("Server read timeout {0}, Beging waiting for another client.", ioe.Message);
				}
				catch (SocketException se)
				{
					Console.WriteLine("Terminating server {0}", se.Message);
					return;
				}
				finally
				{
					if (stream != null)
						stream.Close();

					if (client != null)
						client.Close();
				}				
			}
		}

		//public void Listen()
		//{
		//    Server.Start();
		//    Console.WriteLine("Server waiting for client...");
		//    TcpClient client = Server.AcceptTcpClient();			

		//    // got a client, read the bytes
		//    Console.WriteLine("Server accepted client");
		//    NetworkStream stream = client.GetStream();


		//    int b;

		//    while ((b = stream.ReadByte()) != -1)
		//    {
		//        Console.WriteLine("Server read {0}", b);
		//        Console.WriteLine("Server write {0}", b);
		//        stream.WriteByte((byte) b);
		//    }

		//    stream.Close();
		//    client.Close();
		//}
	}
}
