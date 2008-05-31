using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using Modbus.Message;
using Unme.Common;

namespace Modbus.IO
{
	/// <summary>
	/// Transport for Internet protocols.
	/// </summary>
	abstract class ModbusIpTransport : ModbusTransport
	{
		private ushort _transactionId;
		private static readonly object _transactionIdLock = new object();

		/// <summary>
		/// Create a new transaction ID.
		/// </summary>
		internal virtual ushort GetNewTransactionId()
		{
			lock (_transactionIdLock)
				_transactionId = _transactionId == UInt16.MaxValue ? (ushort) 1 : ++_transactionId;

			return _transactionId;
		}

		internal IModbusMessage CreateMessageAndInitializeTransactionId<T>(byte[] fullFrame) where T : IModbusMessage, new()
		{
			byte[] mbapHeader = fullFrame.Slice(0, 6).ToArray();
			byte[] messageFrame = fullFrame.Slice(6, fullFrame.Length - 6).ToArray();

			IModbusMessage response = base.CreateResponse<T>(messageFrame);
			response.TransactionID = (ushort) IPAddress.NetworkToHostOrder(BitConverter.ToInt16(mbapHeader, 0));

			return response;
		}

		internal static byte[] GetMbapHeader(IModbusMessage message)
		{
			byte[] transactionId = BitConverter.GetBytes((short) IPAddress.HostToNetworkOrder((short) (message.TransactionID)));
			byte[] protocol = { 0, 0 };
			byte[] length = BitConverter.GetBytes((short) IPAddress.HostToNetworkOrder((short) (message.ProtocolDataUnit.Length + 1)));
				
			return transactionId.Concat(protocol, length, new byte[] { message.SlaveAddress }).ToArray();
		}

		internal override byte[] BuildMessageFrame(IModbusMessage message)
		{
			if (message.TransactionID == 0)
				message.TransactionID = GetNewTransactionId();

			List<byte> messageBody = new List<byte>();
			messageBody.AddRange(GetMbapHeader(message));
			messageBody.AddRange(message.ProtocolDataUnit);

			return messageBody.ToArray();
		}

		internal override void ValidateResponse(IModbusMessage request, IModbusMessage response)
		{
			if (request.TransactionID != response.TransactionID)
				throw new IOException(String.Format(CultureInfo.InvariantCulture, "Response was not of expected transaction ID. Expected {0}, received {1}.", request.TransactionID, response.TransactionID));

			base.ValidateResponse(request, response);
		}
	}
}
