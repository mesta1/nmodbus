using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Modbus.Message;
using Modbus.Util;
using Modbus.IO;

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

		public override void  Listen()
		{
			_tcpListener.Start();
			
			// TODO spawn thread for each master communication exchange
			TcpClient master = _tcpListener.AcceptTcpClient();
			NetworkStream masterStream = master.GetStream();
			
			while (true)
			{
				try
				{
					// use transport to retrieve raw message frame from stream
					byte[] frame = ReadRequestResponse(masterStream);

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
				catch (Exception e)
				{
					// TODO explicitly catch timeout exception	
					log.ErrorFormat(ModbusResources.ModbusSlaveListenerException, e.Message);
				}
			}
		}

		public byte[] ReadRequestResponse(NetworkStream stream)
		{
			// read header
			byte[] mbapHeader = new byte[ModbusTcpTransport.MbapHeaderLength];
			int numBytesRead = 0;
			while (numBytesRead != ModbusTcpTransport.MbapHeaderLength)
				numBytesRead += stream.Read(mbapHeader, numBytesRead, ModbusTcpTransport.MbapHeaderLength - numBytesRead);

			ushort frameLength = (ushort) IPAddress.HostToNetworkOrder(BitConverter.ToInt16(mbapHeader, 4));

			// read message
			byte[] frame = new byte[frameLength];
			numBytesRead = 0;
			while (numBytesRead != frameLength)
				numBytesRead += stream.Read(frame, numBytesRead, frameLength - numBytesRead);

			return frame;
		}
	}
}
