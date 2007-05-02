using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using log4net;
using Modbus.IO;
using Modbus.Message;
using Modbus.Util;

namespace Modbus.Device
{
	/// <summary>
	/// Modbus TCP slave device.
	/// </summary>
	public class ModbusTcpSlave : ModbusSlave
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(ModbusTcpSlave));
		private TcpListener _tcpListener;

		private ModbusTcpSlave(byte unitID, TcpListener tcpListener)
			: base(unitID, new ModbusTcpTransport())
		{			
			_tcpListener = tcpListener;
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
			_tcpListener.Start();
			
			_log.Debug("Block for pending client connection.");
			TcpClient master = _tcpListener.AcceptTcpClient();
			_log.Debug("Connected to client."); 
			Transport = new ModbusTcpTransport(new TcpStreamAdapter(master.GetStream()));

			try
			{
				while (true)
				{
					// read request and build message
					byte[] frame = Transport.ReadRequest();
					IModbusMessage request = ModbusMessageFactory.CreateModbusRequest(CollectionUtil.Slice(frame, 6, frame.Length - 6));					
					request.TransactionID = (ushort) IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 0));

					// perform action and build response
					IModbusMessage response = ApplyRequest(request);
					response.TransactionID = request.TransactionID;

					// write response
					Transport.Write(response);
				}
			}
			catch (SocketException se)
			{
				if (se.ErrorCode == Modbus.WSAECONNABORTED)
				{
					_log.Info("Master connection aborted.");
				}
				else
				{
					_log.Error(String.Format("Unexpected Socket Exception - {0}", se.Message));
					throw se;
				}
			}
			finally
			{
				if (master != null)
					master.Close();
			}				
		}
	}
}
