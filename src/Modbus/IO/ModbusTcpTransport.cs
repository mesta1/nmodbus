using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using log4net;
using Modbus.Message;
using Modbus.Util;
using System.Net.Sockets;

namespace Modbus.IO
{
	class ModbusTcpTransport : ModbusTransport
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(ModbusTcpTransport));
		private TcpStreamAdapter _tcpStreamAdapter;
		private ushort _transactionID;
		private static object _transactionIDLock = new object();

		public ModbusTcpTransport()
		{
		}

		public ModbusTcpTransport(TcpStreamAdapter tcpStreamAdapter)
		{
			_tcpStreamAdapter = tcpStreamAdapter;
		}

		public virtual ushort GetNewTransactionID()
		{
			lock (_transactionIDLock)
				_transactionID = _transactionID == UInt16.MaxValue ? (ushort) 1 : ++_transactionID;

			return _transactionID;
		}

		public static byte[] GetMbapHeader(IModbusMessage message)
		{
			byte[] transactionID = BitConverter.GetBytes((short) IPAddress.HostToNetworkOrder((short) (message.TransactionID)));
			byte[] protocol = { 0, 0 };
			byte[] length = BitConverter.GetBytes((short) IPAddress.HostToNetworkOrder((short) (message.ProtocolDataUnit.Length + 1)));

			return CollectionUtil.Combine(transactionID, protocol, length, new byte[] { message.SlaveAddress });
		}

		public static byte[] ReadRequestResponse(TcpStreamAdapter tcpTransportAdapter)
		{
			// read header
			byte[] mbapHeader = new byte[6];
			int numBytesRead = 0;
			while (numBytesRead != 6)
			{
				numBytesRead += tcpTransportAdapter.Read(mbapHeader, numBytesRead, 6 - numBytesRead);

				if (numBytesRead == 0)
					throw new SocketException(Modbus.WSAECONNABORTED);
			}
			_log.DebugFormat("MBAP header: {0}", StringUtil.Join(", ", mbapHeader));

			ushort frameLength = (ushort) (IPAddress.HostToNetworkOrder(BitConverter.ToInt16(mbapHeader, 4)));
			_log.DebugFormat("{0} bytes in PDU.", frameLength);

			// read message
			byte[] messageFrame = new byte[frameLength];
			numBytesRead = 0;
			while (numBytesRead != frameLength)
			{
				numBytesRead += tcpTransportAdapter.Read(messageFrame, numBytesRead, frameLength - numBytesRead);
				
				if (numBytesRead == 0)
					throw new SocketException(Modbus.WSAECONNABORTED);
			}
			_log.DebugFormat("PDU: {0}", frameLength);

			byte[] frame = CollectionUtil.Combine(mbapHeader, messageFrame);
			_log.InfoFormat("RX: {0}", StringUtil.Join(", ", frame));

			return frame;
		}

		internal override void Write(IModbusMessage message)
		{			
			byte[] frame = BuildMessageFrame(message);
			_log.InfoFormat("TX: {0}", StringUtil.Join(", ", frame));
			_tcpStreamAdapter.Write(frame, 0, frame.Length);
		}

		internal override byte[] BuildMessageFrame(IModbusMessage message)
		{
			if (message.TransactionID == 0)
				message.TransactionID = GetNewTransactionID();

			List<byte> messageBody = new List<byte>();
			messageBody.AddRange(GetMbapHeader(message));
			messageBody.AddRange(message.ProtocolDataUnit);

			return messageBody.ToArray();
		}

		internal override byte[] ReadRequest()
		{
			return ReadRequestResponse(_tcpStreamAdapter);
		}		

		internal override T ReadResponse<T>()
		{
			byte[] fullFrame = ReadRequestResponse(_tcpStreamAdapter);
			byte[] mbapHeader = CollectionUtil.Slice(fullFrame, 0, 6);
			byte[] messageFrame = CollectionUtil.Slice(fullFrame, 6, fullFrame.Length - 6);

			T response = base.CreateResponse<T>(messageFrame);
			response.TransactionID = (ushort) IPAddress.NetworkToHostOrder(BitConverter.ToInt16(mbapHeader, 0));

			return response;
		}

		internal override void ValidateResponse(IModbusMessage request, IModbusMessage response)
		{
			if (request.TransactionID != response.TransactionID)
				throw new IOException(String.Format("Response was not of expected transaction ID. Expected {0}, received {1}.", request.TransactionID, response.TransactionID));

			base.ValidateResponse(request, response);
		}	
	}
}
