using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Modbus.Util;
using Modbus.Data;

namespace Modbus.Message
{
	public class ReadCoilsResponse : ModbusMessageWithData<CoilDiscreteCollection>, IModbusMessage
	{
		private const int _minimumFrameSize = 4;

		public ReadCoilsResponse()
		{
		}

		public ReadCoilsResponse(byte slaveAddress, byte byteCount, CoilDiscreteCollection data)
			: base(slaveAddress, Modbus.ReadCoils)
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
				throw new FormatException("Message frame data segment does not contain enough bytes.");

			ByteCount = frame[2];
			Data = new CoilDiscreteCollection(CollectionUtil.Slice<byte>(frame, 3, ByteCount));
		}
	}
}
