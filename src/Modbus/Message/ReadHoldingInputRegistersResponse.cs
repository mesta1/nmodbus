using System;
using Modbus.Data;
using Modbus.Utility;

namespace Modbus.Message
{
	class ReadHoldingInputRegistersResponse : ModbusMessageWithData<RegisterCollection>, IModbusMessage
	{
		private const int _minimumFrameSize = 3;

		public ReadHoldingInputRegistersResponse()
		{
		}

		public ReadHoldingInputRegistersResponse(byte functionCode, byte slaveAddress, RegisterCollection data)
			: base(slaveAddress, functionCode)
		{
			if (data == null)
				throw new ArgumentNullException("data");

			ByteCount = data.ByteCount;
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

		public override string ToString()
		{
			return String.Format("Read {0} {1} registers.", Data.Count, FunctionCode == Modbus.ReadHoldingRegisters ? "holding" : "input");
		}

		protected override void InitializeUnique(byte[] frame)
		{
			if (frame.Length < _minimumFrameSize + frame[2])
				throw new FormatException("Message frame does not contain enough bytes.");

			ByteCount = frame[2];
			Data = new RegisterCollection(CollectionUtility.Slice<byte>(frame, 3, ByteCount));
		}
	}
}
