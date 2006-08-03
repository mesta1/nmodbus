using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Data;
using Modbus.Util;
using System.Net;

namespace Modbus.Message
{
	public class WriteMultipleRegistersRequest : ModbusMessageWithData<HoldingRegisterCollection>, IModbusMessage
	{
		private const int _minimumFrameSize = 7;

		public WriteMultipleRegistersRequest()
		{
		}

		public WriteMultipleRegistersRequest(byte slaveAddress, ushort startAddress, ushort numberOfPoints, byte byteCount, HoldingRegisterCollection data)
			: base(slaveAddress, Modbus.WriteMultipleRegisters)
		{
			StartAddress = startAddress;
			NumberOfPoints = numberOfPoints;
			ByteCount = byteCount;
			Data = data;
		}

		public byte ByteCount
		{
			get { return MessageImpl.ByteCount; }
			set { MessageImpl.ByteCount = value; }
		}

		public ushort NumberOfPoints
		{
			get { return MessageImpl.NumberOfPoints; }
			set { MessageImpl.NumberOfPoints = value; }
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
		
		protected override void InitializeUnique(byte[] frame)
		{
			if (frame.Length < 7 + frame[6])
				throw new FormatException("Message frame does not contain enough bytes.");

			StartAddress = (ushort) IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 2));
			NumberOfPoints = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 4));
			ByteCount = frame[6];
			Data = new HoldingRegisterCollection(CollectionUtil.Slice<byte>(frame, 7, ByteCount));
		}
	}
}
