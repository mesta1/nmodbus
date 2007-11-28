using System;
using System.Collections.Generic;
using System.Net;
using Modbus.Data;

namespace Modbus.Message
{
	/// <summary>
	/// Class holding all implementation shared between two or more message types. 
	/// Interfaces expose subsets of type specific implementations.
	/// </summary>
	class ModbusMessageImpl
	{
		private byte? _exceptionCode;
		private ushort _transactionID;
		private byte _functionCode;
		private ushort? _subFunctionCode;
		private byte _slaveAddress;
		private ushort? _startAddress;
		private ushort? _numberOfPoints;
		private byte? _byteCount;
		private IModbusMessageDataCollection _data;

		public ModbusMessageImpl()
		{
		}

		public ModbusMessageImpl(byte slaveAddress, byte functionCode)
		{
			_slaveAddress = slaveAddress;
			_functionCode = functionCode;
		}

		public byte ByteCount
		{
			get { return _byteCount.Value; }
			set { _byteCount = value; }
		}

		public byte ExceptionCode
		{
			get { return _exceptionCode.Value; }
			set { _exceptionCode = value; }
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

		public ushort NumberOfPoints
		{
			get { return _numberOfPoints.Value; }
			set { _numberOfPoints = value; }
		}

		public byte SlaveAddress
		{
			get { return _slaveAddress; }
			set { _slaveAddress = value; }
		}

		public ushort StartAddress
		{
			get { return _startAddress.Value; }
			set { _startAddress = value; }
		}

		public ushort SubFunctionCode
		{
			get { return _subFunctionCode.Value; }
			set { _subFunctionCode = value; }
		}

		public IModbusMessageDataCollection Data
		{
			get { return _data; }
			set { _data = value; }
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

				if (_exceptionCode.HasValue)
					pdu.Add(_exceptionCode.Value);

				if (_subFunctionCode.HasValue)
					pdu.AddRange(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short) _subFunctionCode.Value)));

				if (_startAddress.HasValue)
					pdu.AddRange(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short) _startAddress.Value)));

				if (_numberOfPoints.HasValue)
					pdu.AddRange(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short) _numberOfPoints.Value)));

				if (_byteCount.HasValue)
					pdu.Add(_byteCount.Value);

				if (_data != null)
					pdu.AddRange(_data.NetworkBytes);

				return pdu.ToArray();
			}
		}

		public void Initialize(byte[] frame)
		{
			if (frame == null)
				throw new ArgumentNullException("frame", "Argument frame cannot be null.");

			if (frame.Length < Modbus.MinimumFrameSize)
				throw new FormatException(String.Format("Message frame must contain at least {0} bytes of data.", Modbus.MinimumFrameSize));

			SlaveAddress = frame[0];
			FunctionCode = frame[1];
		}
	}
}
