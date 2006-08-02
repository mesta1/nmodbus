using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Data;
using System.Net;

namespace Modbus.Message
{
	public class WriteSingleRegisterRequest : ModbusMessageWithData<HoldingRegisterCollection>, IModbusMessage
	{
		private const int _minimumFrameSize = 6;

		public WriteSingleRegisterRequest()
		{
		}

		public WriteSingleRegisterRequest(byte slaveAddress, byte startAddress, ushort registerValue)
			: base(slaveAddress, Modbus.WriteSingleRegister)
		{
			StartAddress = startAddress;
			Data = new HoldingRegisterCollection(registerValue);
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

		protected override void InitializeUnique(byte[] frame)
		{
			StartAddress = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 2));
			Data = new HoldingRegisterCollection((ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 4)));
		}
	}
}
