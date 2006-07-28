using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Modbus.Util;
using Modbus.Data;

namespace Modbus.Message
{
	public class ReadCoilsResponse : ModbusMessageWithData<CoilDiscreteCollection>
	{
		private const int MinFrameSize = 4;

		public ReadCoilsResponse()
		{
		}

		public ReadCoilsResponse(byte slaveAddress, byte byteCount, CoilDiscreteCollection data)
			: base(slaveAddress, Modbus.READ_COILS)
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
			Data = new CoilDiscreteCollection(CollectionUtil.Slice<byte>(frame, 3, ByteCount));
		}
	}
}
