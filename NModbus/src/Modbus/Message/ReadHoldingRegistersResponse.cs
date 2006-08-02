using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Data;
using Modbus.Util;

namespace Modbus.Message
{
	public class ReadHoldingRegistersResponse : ModbusMessageWithData<HoldingRegisterCollection>, IModbusMessage
	{
		private const int _minimumFrameSize = 3;

		public ReadHoldingRegistersResponse()
		{
		}

		public ReadHoldingRegistersResponse(byte slaveAddress, byte byteCount, HoldingRegisterCollection data)
			: base(slaveAddress, Modbus.ReadHoldingRegisters)
		{
			ByteCount = byteCount;
			Data = data;
		}

		public byte ByteCount
		{
			get { return MessageImpl.ByteCount; }
			set { MessageImpl.ByteCount = value; }
		}

		public override int MinimumFrameSize
		{
			get { return _minimumFrameSize; }
		}

		protected override void InitializeUnique(byte[] frame)
		{
			if (frame.Length < 3 + frame[2])
				throw new FormatException("Message frame does not contain enough bytes.");

			ByteCount = frame[2];
			Data = new HoldingRegisterCollection(CollectionUtil.Slice<byte>(frame, 3, ByteCount));
		}
	}
}
