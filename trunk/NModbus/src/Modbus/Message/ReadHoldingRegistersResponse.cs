using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Data;
using Modbus.Util;

namespace Modbus.Message
{
	public class ReadHoldingRegistersResponse : ModbusMessageWithData<HoldingRegisterCollection>
	{
		private const int MinFrameSize = 3;

		public ReadHoldingRegistersResponse()
		{
		}

		public ReadHoldingRegistersResponse(byte slaveAddress, byte byteCount, HoldingRegisterCollection data)
			: base(slaveAddress, Modbus.READ_HOLDING_REGISTERS)
		{
			ByteCount = byteCount;
			Data = data;
		}

		public byte ByteCount
		{
			get { return MessageImpl.ByteCount; }
			set { MessageImpl.ByteCount = value; }
		}

		protected override void InitializeUnique(byte[] frame)
		{
			if (frame.Length < MinFrameSize)
				throw new FormatException(String.Format("Message frame must contain at least {0} bytes of data.", MinFrameSize));

			if (frame.Length < 3 + frame[2])
				throw new FormatException("Message frame does not contain enough bytes.");

			ByteCount = frame[2];
			Data = new HoldingRegisterCollection(CollectionUtil.Slice<byte>(frame, 3, ByteCount));
		}
	}
}
