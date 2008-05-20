using System;
using System.Collections.Generic;
using System.Linq;
using Modbus.Data;
using Modbus.Message;
using Unme.Common;

namespace Modbus.IntegrationTests.CustomMessages
{
	public class CustomReadHoldingRegistersResponse : IModbusMessageWithData<ushort>
	{
		private byte _functionCode;
		private byte _slaveAddress;
		private byte _byteCount;
		private ushort _transactionID;
		private RegisterCollection _data;

		public ushort[] Data
		{
			get
			{
				return _data.ToArray();
			}
		}

		public byte[] MessageFrame
		{
			get
			{
				List<byte> frame = new List<byte>();
				frame.Add(SlaveAddress);
				frame.AddRange(ProtocolDataUnit);

				return frame.ToArray();
			}
		}

		public byte[] ProtocolDataUnit
		{
			get
			{
				List<byte> pdu = new List<byte>();

				pdu.Add(_functionCode);
				pdu.Add(ByteCount);
				pdu.AddRange(_data.NetworkBytes);

				return pdu.ToArray();
			}
		}

		public ushort TransactionID
		{
			get { return _transactionID; }
			set { _transactionID = value; }
		}

		public byte FunctionCode
		{
			get { return _functionCode; }
			set { _functionCode = value; }
		}

		public byte SlaveAddress
		{
			get { return _slaveAddress; }
			set { _slaveAddress = value; }
		}

		public byte ByteCount
		{
			get
			{
				return _byteCount;
			}
			set { _byteCount = value; }
		}

		public void Initialize(byte[] frame)
		{
			if (frame == null)
				throw new ArgumentNullException("frame");

			if (frame.Length < 3 || frame.Length < 3 + frame[2])
				throw new ArgumentException("Message frame does not contain enough bytes.", "frame");

			SlaveAddress = frame[0];
			FunctionCode = frame[1];
			ByteCount = frame[2];
			_data = new RegisterCollection(frame.Slice(3, ByteCount).ToArray());
		}
	}
}
