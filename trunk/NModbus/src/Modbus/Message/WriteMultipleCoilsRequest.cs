using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Data;
using System.Net;
using Modbus.Util;

namespace Modbus.Message
{
	public class WriteMultipleCoilsRequest : ModbusMessageWithData<CoilDiscreteCollection>, IModbusMessage
	{
		private const int _minimumFrameSize = 7;

		public WriteMultipleCoilsRequest()
		{
		}

		public WriteMultipleCoilsRequest(byte slaveAddress, ushort startAddress, CoilDiscreteCollection data)
			: base(slaveAddress, Modbus.WriteMultipleCoils)
		{
			StartAddress = startAddress;
			NumberOfPoints = (ushort) data.Count;
			ByteCount = (byte)(data.Count / 8 + (data.Count % 8 > 0 ? 1 : 0));
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
			if (frame.Length < _minimumFrameSize + frame[6])
				throw new FormatException("Message frame does not contain enough bytes.");

			StartAddress = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 2));
			NumberOfPoints = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 4));
			ByteCount = frame[6];
			Data = new CoilDiscreteCollection(CollectionUtil.Slice<byte>(frame, 7, ByteCount));
		}
	}
}
