using System;
using System.Collections.Generic;
using System.Text;

namespace Modbus.Message
{
	public class WriteSingleCoilRequest : ModbusMessage, IModbusMessage
	{
		public WriteSingleCoilRequest(byte slaveAddress, ushort coilAddress, ushort coilState)
			: base(slaveAddress, Modbus.WriteSingleCoil)
		{
			StartAddress = coilAddress;
		}

		public ushort StartAddress
		{
			get { return MessageImpl.StartAddress; }
			set { MessageImpl.StartAddress = value; }
		}

		protected override void InitializeUnique(byte[] frame)
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
