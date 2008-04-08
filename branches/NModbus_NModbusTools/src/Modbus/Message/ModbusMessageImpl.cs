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
		private ushort? _subFunctionCode;
		private ushort? _startAddress;
		private ushort? _numberOfPoints;
		private byte? _byteCount;

		public ModbusMessageImpl()
		{
		}

		public ModbusMessageImpl(byte slaveAddress, byte functionCode)
		{
			SlaveAddress = slaveAddress;
			FunctionCode = functionCode;
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

		public ushort TransactionID { get; set; }
		
		public byte FunctionCode { get; set; }

		public ushort NumberOfPoints
		{
			get { return _numberOfPoints.Value; }
			set { _numberOfPoints = value; }
		}

		public byte SlaveAddress { get; set; }

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

		public IModbusMessageDataCollection Data { get; set; }

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

				pdu.Add(FunctionCode);

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

				if (Data != null)
					pdu.AddRange(Data.NetworkBytes);

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
