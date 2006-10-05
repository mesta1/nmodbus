using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Modbus.Message;
using Modbus.Util;
using Modbus.IO;
using System.Threading;

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
			_tcpListener.Start();
					
			while (true)
			{
				try
				{
					// TODO spawn thread for each master communication exchange
					TcpClient master = _tcpListener.AcceptTcpClient();
					Thread workerThread = new Thread(TcpConnectionHandler);
					workerThread.Start(master);
				}
				catch (Exception)
				{
					
				}				
			}
		}

		public void TcpConnectionHandler(object master)
		{
			NetworkStream masterStream = ((TcpClient) master).GetStream();

			while (true)
			{
				// use transport to retrieve raw message frame from stream
				byte[] frame = ModbusTcpTransport.ReadRequestResponse(masterStream);

				// build request from frame
				IModbusMessage request = ModbusMessageFactory.CreateModbusRequest(frame);
				log.DebugFormat("RX: {0}", StringUtil.Join(", ", request.MessageFrame));

				// perform action
				IModbusMessage response = ApplyRequest(request);

				// write response				
				byte[] responseFrame = new ModbusTcpTransport().BuildMessageFrame(response);
				log.DebugFormat("TX: {0}", StringUtil.Join(", ", responseFrame));
				masterStream.Write(responseFrame, 0, responseFrame.Length);
			}
		}
	}
}
