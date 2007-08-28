using System;
using System.Net;
using Modbus.Data;

namespace Modbus.Message
{
	class WriteSingleRegisterRequestResponse : ModbusMessageWithData<RegisterCollection>, IModbusMessage
	{
		private const int _minimumFrameSize = 6;

		public WriteSingleRegisterRequestResponse()
		{
		}

		public WriteSingleRegisterRequestResponse(byte slaveAddress, ushort startAddress, ushort registerValue)
			: base(slaveAddress, Modbus.WriteSingleRegister)
		{
			StartAddress = startAddress;
			Data = new RegisterCollection(registerValue);
		}

		public override int MinimumFrameSize
		{
			get { return _minimumFrameSize; }
		}

		public ushort StartAddress
		{
			get { return MessageImpl.StartAddress; }
			set { MessageImpl.StartAddress = value; }
		}

		public override string ToString()
		{
			return String.Format("Write single holding register at address {0}.", StartAddress);
		}

		protected override void InitializeUnique(byte[] frame)
		{
			StartAddress = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 2));
			Data = new RegisterCollection((ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 4)));
		}
	}
}
