using System;
using System.Net;
using Modbus.Data;
using Modbus.Utility;

namespace Modbus.Message
{
	class WriteMultipleRegistersRequest : ModbusMessageWithData<RegisterCollection>, IModbusMessage
	{
		private const int _minimumFrameSize = 7;

		public WriteMultipleRegistersRequest()
		{
		}

		public WriteMultipleRegistersRequest(byte slaveAddress, ushort startAddress, RegisterCollection data)
			: base(slaveAddress, Modbus.WriteMultipleRegisters)
		{
			StartAddress = startAddress;
			NumberOfPoints = (ushort) data.Count;
			ByteCount = (byte) (data.Count * 2);
			Data = data;
		}

		public byte ByteCount
		{
			get { return MessageImpl.ByteCount; }
			set { MessageImpl.ByteCount = value; }
		}

		public ushort NumberOfPoints
		{
			get
			{
				return MessageImpl.NumberOfPoints;
			}
			set
			{
				if (value > Modbus.MaximumRegisterRequestResponseSize)
					throw new ArgumentOutOfRangeException("NumberOfPoints", String.Format("Maximum amount of data {0} registers.", Modbus.MaximumRegisterRequestResponseSize));

				MessageImpl.NumberOfPoints = value;
			}
		}
		
		public ushort StartAddress
		{
			get { return MessageImpl.StartAddress; }
			set { MessageImpl.StartAddress = value; }
		}

		public override int MinimumFrameSize
		{
			get { return _minimumFrameSize; }
		}

		public override string ToString()
		{
			return String.Format("Write {0} holding registers at address {1}.", NumberOfPoints, StartAddress);
		}

		protected override void InitializeUnique(byte[] frame)
		{
			if (frame.Length < _minimumFrameSize + frame[6])
				throw new FormatException("Message frame does not contain enough bytes.");

			StartAddress = (ushort) IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 2));
			NumberOfPoints = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 4));
			ByteCount = frame[6];
			Data = new RegisterCollection(CollectionUtility.Slice<byte>(frame, 7, ByteCount));
		}
	}
}
