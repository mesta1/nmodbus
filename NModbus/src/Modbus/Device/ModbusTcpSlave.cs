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

namespace Modbus.Device
{
	public class ModbusTcpSlave : ModbusSlave
	{
		private TcpListener _tcpListener;

		private ModbusTcpSlave(byte unitID, TcpListener tcpListener)
			: base(unitID, new ModbusTcpTransport())
		{			
			_tcpListener = tcpListener;
		}

		public static ModbusTcpSlave CreateTcp(byte unitID, TcpListener tcpListener)
		{
			return new ModbusTcpSlave(unitID, tcpListener);
		}

		public override void Listen()
		{
			log.Debug("Start Modbus Tcp Server.");
			_tcpListener.Start();
			
			log.Debug("Block for pending client connection.");
			TcpClient master = _tcpListener.AcceptTcpClient();
			log.Debug("Connected to client.");
			NetworkStream stream = master.GetStream();

			try
			{
				while (true)
				{
					// use transport to retrieve raw message frame from stream
					byte[] frame = ModbusTcpTransport.ReadRequestResponse(stream);

					// build request from frame
					IModbusMessage request = ModbusMessageFactory.CreateModbusRequest(frame);
					log.DebugFormat("RX: {0}", StringUtil.Join(", ", request.MessageFrame));

					// perform action
					IModbusMessage response = ApplyRequest(request);

					// write response				
					byte[] responseFrame = new ModbusTcpTransport().BuildMessageFrame(response);
					log.DebugFormat("TX: {0}", StringUtil.Join(", ", responseFrame));
					stream.Write(responseFrame, 0, responseFrame.Length);
				}
			}
			catch (SocketException se)
			{
				log.ErrorFormat("Terminating Modbus Tcp server - {0}, ", se.Message);
				return;
			}
			catch (Exception e)
			{
				log.ErrorFormat("Unexpected exception - {0}", e.Message);
				return;
			}
			finally
			{
				if (stream != null)
					stream.Close();

				if (master != null)
					master.Close();
			}				
		}
	}
}
