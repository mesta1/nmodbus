using log4net;
using Modbus.IO;
using Modbus.Message;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Modbus.Utility;

namespace Modbus.Device
{
	/// <summary>
	/// Modbus TCP slave device.
	/// </summary>
	public class ModbusTcpSlave : ModbusSlave
	{
		private static readonly object _mastersLock = new object();
		private static readonly ILog _log = LogManager.GetLogger(typeof(ModbusTcpSlave));
		private static readonly Dictionary<string, TcpClient> _masters = new Dictionary<string, TcpClient>();
		private readonly TcpListener _server;

		private ModbusTcpSlave(byte unitID, TcpListener tcpListener)
			: base(unitID, new ModbusTcpTransport())
		{
			_server = tcpListener;
		}

		/// <summary>
		/// Gets the Modbus TCP Masters connected to this Modbus TCP Slave.
		/// </summary>
		public static ReadOnlyCollection<TcpClient> Masters
		{
			get
			{
				lock (_mastersLock)
					return new ReadOnlyCollection<TcpClient>(SequenceUtility.ToList(_masters.Values));
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

		internal static void RemoveMaster(string endPoint)
		{
			lock (_mastersLock)
			{
				if (!_masters.Remove(endPoint))
					throw new ArgumentException(String.Format("EndPoint {0} cannot be removed, it does not exist.", endPoint));
			}

			_log.InfoFormat("Removed Master {0}", endPoint);
		}

		internal void AcceptCompleted(IAsyncResult ar)
		{
			ModbusTcpSlave slave = (ModbusTcpSlave) ar.AsyncState;

			try
			{
				TcpClient client = _server.EndAcceptTcpClient(ar);

				lock (_mastersLock)
					_masters.Add(client.Client.RemoteEndPoint.ToString(), client);

				new MasterConnection(client.Client.RemoteEndPoint.ToString(), client.GetStream(), slave);
				_log.Debug("Accept completed.");

				// Accept another client
				_server.BeginAcceptTcpClient(AcceptCompleted, slave);
			}
			catch (ObjectDisposedException)
			{
				// this happens when the server stops
			}
		}

		// TODO all read completed methods need to ensure all the data has been read, else they need to try to read again
		// TODO really we should wrap any BeginXXX or EndXXX in a try catch block to make the slave more robust...
		internal class MasterConnection
		{
			private ModbusTcpSlave _slave;
			private string _endPoint;
			private NetworkStream _stream;
			private byte[] _mbapHeader = new byte[6];
			private byte[] _messageFrame;

			public MasterConnection(string endPoint, NetworkStream stream, ModbusTcpSlave slave)
			{
				if (stream == null)
					throw new ArgumentException("stream");

				if (slave == null)
					throw new ArgumentException("slave");

				_log.DebugFormat("Creating new Master connection at IP:{0}", endPoint);
				_slave = slave;
				_endPoint = endPoint;
				_stream = stream;

				_log.Debug("Begin reading header.");
				_stream.BeginRead(_mbapHeader, 0, 6, ReadHeaderCompleted, null);
			}

			internal void ReadHeaderCompleted(IAsyncResult ar)
			{
				_log.Debug("Read header completed.");

				try
				{
					if (_stream.EndRead(ar) == 0)
					{
						_log.Debug("0 bytes read, Master has closed Socket connection.");
						RemoveMaster(_endPoint);
						return;
					}
				}
				catch (IOException ioe)
				{
					_log.DebugFormat("IOException encountered in ReadHeaderCompleted - {0}", ioe.Message);
					RemoveMaster(_endPoint);

					SocketException socketException = ioe.InnerException as SocketException;
					if (socketException != null && socketException.ErrorCode == Modbus.ConnectionResetByPeer)
					{
						_log.Debug("Socket Exceptiong ConnectionResetByPeer, Master closed connection.");
						return;
					}

					throw;
				}

				_log.DebugFormat("MBAP header: {0}", StringUtility.Join(", ", _mbapHeader));
				ushort frameLength = (ushort) (IPAddress.HostToNetworkOrder(BitConverter.ToInt16(_mbapHeader, 4)));
				_log.DebugFormat("{0} bytes in PDU.", frameLength);
				_messageFrame = new byte[frameLength];

				_stream.BeginRead(_messageFrame, 0, frameLength, ReadFrameCompleted, null);
			}

			internal void ReadFrameCompleted(IAsyncResult ar)
			{
				_log.DebugFormat("Read Frame completed {0} bytes", _stream.EndRead(ar));
				byte[] frame = CollectionUtility.Concat(_mbapHeader, _messageFrame);
				_log.InfoFormat("RX: {0}", StringUtility.Join(", ", frame));

				IModbusMessage request = ModbusMessageFactory.CreateModbusRequest(CollectionUtility.Slice(frame, 6, frame.Length - 6));
				request.TransactionID = (ushort) IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 0));

				// TODO refactor
				ModbusTcpTransport transport = new ModbusTcpTransport();
				// perform action and build response
				IModbusMessage response = _slave.ApplyRequest(request);
				response.TransactionID = request.TransactionID;

				// write response
				byte[] responseFrame = transport.BuildMessageFrame(response);
				_log.InfoFormat("TX: {0}", StringUtility.Join(", ", responseFrame));
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
					_log.Debug("Master has closed Socket connection.");
					RemoveMaster(_endPoint);
				}
			}
		}
	}
}
