using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using log4net;
using Modbus.Message;
using Modbus.Util;

namespace Modbus.IO
{
	class ModbusTcpTransport : ModbusTransport
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(ModbusTcpTransport));
		private readonly TcpStreamAdapter _tcpStreamAdapter;
		private ushort _transactionId;
		private static readonly object _transactionIdLock = new object();

		public ModbusTcpTransport()
		{
		}

		public ModbusTcpTransport(TcpStreamAdapter tcpStreamAdapter)
		{
			_tcpStreamAdapter = tcpStreamAdapter;
		}

		public virtual ushort GetNewTransactionID()
		{
			lock (_transactionIdLock)
				_transactionId = _transactionId == UInt16.MaxValue ? (ushort) 1 : ++_transactionId;

			return _transactionId;
		}

		public static byte[] GetMbapHeader(IModbusMessage message)
		{
			byte[] transactionID = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short) (message.TransactionID)));
			byte[] protocol = { 0, 0 };
			byte[] length = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short) (message.ProtocolDataUnit.Length + 1)));

			return CollectionUtility.Concat(transactionID, protocol, length, new byte[] { message.SlaveAddress });
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
					throw new SocketException(Modbus.ConnectionAborted);
			}
			_log.DebugFormat("MBAP header: {0}", StringUtility.Join(", ", mbapHeader));

			ushort frameLength = (ushort) (IPAddress.HostToNetworkOrder(BitConverter.ToInt16(mbapHeader, 4)));
			_log.DebugFormat("{0} bytes in PDU.", frameLength);

			// read message
			byte[] messageFrame = new byte[frameLength];
			numBytesRead = 0;
			while (numBytesRead != frameLength)
			{
				numBytesRead += tcpTransportAdapter.Read(messageFrame, numBytesRead, frameLength - numBytesRead);
				
				if (numBytesRead == 0)
					throw new SocketException(Modbus.ConnectionAborted);
			}
			_log.DebugFormat("PDU: {0}", frameLength);

			byte[] frame = CollectionUtility.Concat(mbapHeader, messageFrame);
			_log.InfoFormat("RX: {0}", StringUtility.Join(", ", frame));

			return frame;
		}

		internal override void Write(IModbusMessage message)
		{			
			byte[] frame = BuildMessageFrame(message);
			_log.InfoFormat("TX: {0}", StringUtility.Join(", ", frame));			
			_tcpStreamAdapter.BeginWrite(frame, 0, frame.Length, WriteCompleted, _tcpStreamAdapter);
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

		internal override IModbusMessage ReadResponse<T>()
		{
			byte[] fullFrame = ReadRequestResponse(_tcpStreamAdapter);
			byte[] mbapHeader = CollectionUtility.Slice(fullFrame, 0, 6);
			byte[] messageFrame = CollectionUtility.Slice(fullFrame, 6, fullFrame.Length - 6);

			IModbusMessage response = base.CreateResponse<T>(messageFrame);
			response.TransactionID = (ushort) IPAddress.NetworkToHostOrder(BitConverter.ToInt16(mbapHeader, 0));

			return response;
		}

		internal override void ValidateResponse(IModbusMessage request, IModbusMessage response)
		{
			if (request.TransactionID != response.TransactionID)
				throw new IOException(String.Format("Response was not of expected transaction ID. Expected {0}, received {1}.", request.TransactionID, response.TransactionID));

			base.ValidateResponse(request, response);
		}

		internal static void WriteCompleted(IAsyncResult ar)
		{
			_log.Debug("Write completed.");
			TcpStreamAdapter stream = (TcpStreamAdapter) ar.AsyncState;
			stream.EndWrite(ar);
		}
	}
}
