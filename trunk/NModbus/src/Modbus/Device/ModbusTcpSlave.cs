using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Modbus.Message;
using Modbus.Util;
using Modbus.IO;
using System.Threading;
using System.IO;
using log4net;

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
			TcpStreamAdapter tcpTransportAdapter = new TcpStreamAdapter(master.GetStream());

			try
			{
				while (true)
				{
					// use transport to retrieve raw message frame from stream
					byte[] frame = ModbusTcpTransport.ReadRequestResponse(tcpTransportAdapter);

					// build request from frame
					IModbusMessage request = ModbusMessageFactory.CreateModbusRequest(frame);
					_log.DebugFormat("RX: {0}", StringUtil.Join(", ", request.MessageFrame));

					// perform action
					IModbusMessage response = ApplyRequest(request);

					// write response
					byte[] responseFrame = new ModbusTcpTransport().BuildMessageFrame(response);
					_log.DebugFormat("TX: {0}", StringUtil.Join(", ", responseFrame));
					tcpTransportAdapter.Write(responseFrame, 0, responseFrame.Length);
				}
			}
			catch (ThreadAbortException)
			{
				_log.Info("NModbus slave thread aborted.");
			}
			catch (Exception e)
			{
				_log.ErrorFormat("Unexpected exception - {0}", e.Message);
			}
			finally
			{
				if (tcpTransportAdapter != null)
					tcpTransportAdapter.Close();

				if (master != null)
					master.Close();
			}				
		}
	}
}
