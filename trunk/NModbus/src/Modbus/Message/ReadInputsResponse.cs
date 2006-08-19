using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Data;
using Modbus.Util;

namespace Modbus.Message
{
	class ReadInputsResponse : ModbusMessageWithData<InputDiscreteCollection>, IModbusMessage
	{
		private const int _minimumFrameSize = 3;

		public ReadInputsResponse()
		{
		}

		public ReadInputsResponse(byte slaveAddress, byte byteCount, InputDiscreteCollection data)
			: base(slaveAddress, Modbus.ReadInputs)
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
			Data = new InputDiscreteCollection(CollectionUtil.Slice<byte>(frame, 3, ByteCount));
		}
	}
}
