using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using log4net;
using Modbus.IO;
using Modbus.Message;
using Modbus.Util;
using System.Collections.Generic;
using System.IO;

namespace Modbus.Device
{
	/// <summary>
	/// Modbus TCP slave device.
	/// </summary>
	public class ModbusTcpSlave : ModbusSlave
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(ModbusTcpSlave));
		private TcpListener _server;

		private ModbusTcpSlave(byte unitID, TcpListener tcpListener)
			: base(unitID, new ModbusTcpTransport())
		{
			_server = tcpListener;
		}

		/// <summary>
		/// Listens for connections from modbus masters (clients).
		/// </summary>
		/// <value>The server.</value>
		public TcpListener Server
		{
			get
			{
				return _server;
			}
		}

		/// <summary>
		/// Modbus TCP slave factory method.
		/// </summary>
		public static ModbusTcpSlave CreateTcp(byte unitID, TcpListener tcpListener)
		{
			return new ModbusTcpSlave(unitID, tcpListener);
		}

		/// <summary>
		/// Start slave listening for requests.
		/// </summary>
		public override void Listen()
		{
			_log.Debug("Start Modbus Tcp Server.");
			_server.Start();

			_server.BeginAcceptTcpClient(AcceptCompleted, this);
		}

		private static void AcceptCompleted(IAsyncResult ar)
		{		
			_log.Debug("Accept completed.");
			ModbusTcpSlave slave = (ModbusTcpSlave) ar.AsyncState;

			try
			{				
				TcpClient client = slave.Server.EndAcceptTcpClient(ar);
				new ClientConnection(slave, client);

				// Accept another client
				slave.Server.BeginAcceptTcpClient(AcceptCompleted, slave);
			}
			catch (ObjectDisposedException)
			{
				// this happens when the server stops
			}
		}

		// TODO all read completed methods need to ensure all the data has been read, else they need to try to read again
		internal class ClientConnection
		{
			private static Dictionary<string, ClientConnection> _masters = new Dictionary<string, ClientConnection>();

			private ModbusTcpSlave _slave;
			private TcpClient _master;
			private NetworkStream _stream;
			byte[] _mbapHeader = new byte[6];
			byte[] _messageFrame;

			public ClientConnection(ModbusTcpSlave slave, TcpClient client)
			{
				_log.DebugFormat("Creating new client connection at IP:{0}", client.Client.RemoteEndPoint);

				_slave = slave;
				_master = client;
				_stream = client.GetStream();
				_masters.Add(client.Client.RemoteEndPoint.ToString(), this);				
				
				_log.Debug("Begin reading header.");
				_stream.BeginRead(_mbapHeader, 0, 6, ReadHeaderCompleted, null);
			}

			private void ReadHeaderCompleted(IAsyncResult ar)
			{
				_log.Debug("Read header completed.");

				try
				{
					_stream.EndRead(ar);
				}
				catch (IOException ioe)
				{
					_log.DebugFormat("Exception encountered in ReadHeaderCompleted - {0}", ioe.Message);

					SocketException socketException = ioe.InnerException as SocketException;
					if (socketException != null && socketException.ErrorCode == Modbus.ConnectionResetByPeer)
					{
						// client closed connection
						_log.Debug("Client closed connection, removing from Master list.");
						_masters.Remove(_master.Client.RemoteEndPoint.ToString());
						return;
					}

					// not sure what happened
					throw;
				}
				
				_log.DebugFormat("MBAP header: {0}", StringUtil.Join(", ", _mbapHeader));
				ushort frameLength = (ushort) (IPAddress.HostToNetworkOrder(BitConverter.ToInt16(_mbapHeader, 4)));
				_log.DebugFormat("{0} bytes in PDU.", frameLength);
				_messageFrame = new byte[frameLength];

				_stream.BeginRead(_messageFrame, 0, frameLength, ReadFrameCompleted, null);
			}

			private void ReadFrameCompleted(IAsyncResult ar)
			{
				_stream.EndRead(ar);

				byte[] frame = CollectionUtil.Combine(_mbapHeader, _messageFrame);
				_log.InfoFormat("RX: {0}", StringUtil.Join(", ", frame));

				IModbusMessage request = ModbusMessageFactory.CreateModbusRequest(CollectionUtil.Slice(frame, 6, frame.Length - 6));
				request.TransactionID = (ushort) IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 0));

				// TODO refactor
				ModbusTcpTransport transport = new ModbusTcpTransport();
				// perform action and build response
				IModbusMessage response = _slave.ApplyRequest(request);
				response.TransactionID = request.TransactionID;

				// write response
				byte[] responseFrame = transport.BuildMessageFrame(response);
				_log.InfoFormat("TX: {0}", StringUtil.Join(", ", responseFrame));
				_stream.BeginWrite(responseFrame, 0, responseFrame.Length, WriteCompleted, null);
			}

			internal void WriteCompleted(IAsyncResult ar)
			{
				_log.Debug("End write.");
				_stream.EndWrite(ar);

				try
				{
					_stream.BeginRead(_mbapHeader, 0, 6, ReadHeaderCompleted, null);
				}
				catch (IOException)
				{
					// client closed connection
					_log.Debug("Client has closed connection.");
					_masters.Remove(_master.Client.RemoteEndPoint.ToString());
				}
			}
		}
	}
}
