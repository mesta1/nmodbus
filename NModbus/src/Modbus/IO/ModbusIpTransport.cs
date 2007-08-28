using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Modbus.Message;
using Modbus.Utility;

namespace Modbus.IO
{
	/// <summary>
	/// Transport for Internet protocols.
	/// </summary>
	abstract class ModbusIpTransport : ModbusTransport
	{
		private ushort _transactionID;
		private static readonly object _transactionIDLock = new object();

		/// <summary>
		/// Create a new transaction ID.
		/// </summary>
		internal virtual ushort GetNewTransactionID()
		{
			lock (_transactionIDLock)
				_transactionID = _transactionID == UInt16.MaxValue ? (ushort) 1 : ++_transactionID;

			return _transactionID;
		}

		internal IModbusMessage CreateMessageAndInitializeTransactionID<T>(byte[] fullFrame) where T : IModbusMessage, new()
		{
			byte[] mbapHeader = CollectionUtility.Slice(fullFrame, 0, 6);
			byte[] messageFrame = CollectionUtility.Slice(fullFrame, 6, fullFrame.Length - 6);

			IModbusMessage response = base.CreateResponse<T>(messageFrame);
			response.TransactionID = (ushort) IPAddress.NetworkToHostOrder(BitConverter.ToInt16(mbapHeader, 0));

			return response;
		}

		internal static byte[] GetMbapHeader(IModbusMessage message)
		{
			byte[] transactionID = BitConverter.GetBytes((short) IPAddress.HostToNetworkOrder((short) (message.TransactionID)));
			byte[] protocol = { 0, 0 };
			byte[] length = BitConverter.GetBytes((short) IPAddress.HostToNetworkOrder((short) (message.ProtocolDataUnit.Length + 1)));

			return CollectionUtility.Concat(transactionID, protocol, length, new byte[] { message.SlaveAddress });
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

		internal override void ValidateResponse(IModbusMessage request, IModbusMessage response)
		{
			if (request.TransactionID != response.TransactionID)
				throw new IOException(String.Format("Response was not of expected transaction ID. Expected {0}, received {1}.", request.TransactionID, response.TransactionID));

			base.ValidateResponse(request, response);
		}
	}
}
