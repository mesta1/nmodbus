using System;
using Modbus.Data;
using Modbus.Utility;

namespace Modbus.Message
{	
	class ReadCoilsInputsResponse : ModbusMessageWithData<DiscreteCollection>, IModbusMessage
	{
		private const int _minimumFrameSize = 3;

		public ReadCoilsInputsResponse()
		{
		}

		public ReadCoilsInputsResponse(byte functionCode, byte slaveAddress, byte byteCount, DiscreteCollection data)
			: base(slaveAddress, functionCode)
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
			Data = new DiscreteCollection(CollectionUtility.Slice<byte>(frame, 3, ByteCount));
		}
	}
}
