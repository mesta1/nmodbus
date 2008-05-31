using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using log4net;
using Modbus.IO;
using Unme.Common;
using Unme.Common.NullReferenceExtension;

namespace Modbus.Device
{
	/// <summary>
	/// Modbus TCP slave device.
	/// </summary>
	public class ModbusTcpSlave : ModbusSlave, IDisposable
	{		
		private readonly object _mastersLock = new object();
		private readonly ILog _log = LogManager.GetLogger(typeof(ModbusTcpSlave));
		private readonly Dictionary<string, ModbusMasterTcpConnection> _masters = new Dictionary<string, ModbusMasterTcpConnection>();
		private readonly TcpListener _server;

		private ModbusTcpSlave(byte unitId, TcpListener tcpListener)
			: base(unitId, new ModbusTcpTransport())
		{
			_server = tcpListener;
		}		

		/// <summary>
		/// Modbus TCP slave factory method.
		/// </summary>
		public static ModbusTcpSlave CreateTcp(byte unitId, TcpListener tcpListener)
		{
			return new ModbusTcpSlave(unitId, tcpListener);
		}

		/// <summary>
		/// Gets the Modbus TCP Masters connected to this Modbus TCP Slave.
		/// </summary>
		public ReadOnlyCollection<TcpClient> Masters
		{
			get
			{
				lock (_mastersLock)
					return new ReadOnlyCollection<TcpClient>(_masters.Values.Select(mc => mc.TcpClient).ToList());
			}
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

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			_masters.IfNotNull(m => m.Values.ForEach(client => DisposableUtility.Dispose(ref client)));
		}

		internal void RemoveMaster(string endPoint)
		{
			lock (_mastersLock)
			{
				if (!_masters.Remove(endPoint))
					throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "EndPoint {0} cannot be removed, it does not exist.", endPoint));
			}

			_log.InfoFormat("Removed Master {0}", endPoint);
		}

		internal void AcceptCompleted(IAsyncResult ar)
		{
			ModbusTcpSlave slave = (ModbusTcpSlave) ar.AsyncState;

			try
			{
				TcpClient client = _server.EndAcceptTcpClient(ar);
				var masterConnection = new ModbusMasterTcpConnection(client, slave);
				masterConnection.ModbusMasterTcpConnectionClosed += (sender, eventArgs) => RemoveMaster(eventArgs.EndPoint);

				lock (_mastersLock)
					_masters.Add(client.Client.RemoteEndPoint.ToString(), masterConnection);
				
				_log.Debug("Accept completed.");

				// Accept another client
				_server.BeginAcceptTcpClient(AcceptCompleted, slave);
			}
			catch (ObjectDisposedException)
			{
				// this happens when the server stops
			}
		}
	}
}
