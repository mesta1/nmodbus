using Modbus.Util;
using System;
using System.Collections.Generic;
using System.Collections;
using Modbus.Data;
using System.Net;
namespace Modbus.Message
{
	/// <summary>
	/// Abstract class holding all implementation shared between two or more message types. 
	/// Interfaces expose subsets of type specific implementations.
	/// </summary>
	public class ModbusMessageImpl
	{
		private byte _functionCode;
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

		public IModbusMessageDataCollection Data
		{
			get { return _data; }
			set { _data = value; }
		}

		public byte[] ChecksumBody
		{
			get
			{
				List<byte> errorCheckBody = new List<byte>();
				errorCheckBody.Add(_slaveAddress);
				errorCheckBody.AddRange(ProtocolDataUnit);

				return errorCheckBody.ToArray();
			}
		}

		public byte[] ProtocolDataUnit
		{
			get
			{
				List<byte> pdu = new List<byte>();
				
				pdu.Add(_functionCode);

				if (_byteCount != null)
					pdu.Add(_byteCount.Value);

				if (_startAddress != null)
					pdu.AddRange(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short) _startAddress.Value)));

				if (_numberOfPoints != null)
					pdu.AddRange(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short) _numberOfPoints.Value)));

				if (_data != null)
					pdu.AddRange(_data.Bytes);

				return pdu.ToArray();
			}
		}

		public void Initialize(byte[] frame)
		{
			if (frame == null)
				throw new ArgumentNullException("frame", "Argument frame cannot be null.");

			if (frame.Length < 2)
				throw new FormatException("Message frame must contain at least two bytes of data.");

			SlaveAddress = frame[0];
			FunctionCode = frame[1];
		}
	}
}
