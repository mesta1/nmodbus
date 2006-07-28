using System;
using System.Collections.Generic;
using System.Text;

namespace Modbus.Message
{
	public class ReadHoldingRegistersRequest : ModbusMessage, IModbusMessage
	{
		private const int MinFrameSize = 6;

		public ReadHoldingRegistersRequest()
		{
		}

		public ReadHoldingRegistersRequest(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
			: base(slaveAddress, Modbus.READ_HOLDING_REGISTERS)
		{
			StartAddress = startAddress;
			NumberOfPoints = numberOfPoints;
		}

		public ushort StartAddress
		{
			get { return MessageImpl.StartAddress; }
			set { MessageImpl.StartAddress = value; }
		}

		public ushort NumberOfPoints
		{
			get { return MessageImpl.NumberOfPoints; }
			set { MessageImpl.NumberOfPoints = value; }
		}

		protected override void InitializeUnique(byte[] frame)
		{
			if (frame.Length < MinFrameSize)
				throw new FormatException(String.Format("Message frame must contain at least {0} bytes of data.", MinFrameSize));

			StartAddress = BitConverter.ToUInt16(frame, 2);
			NumberOfPoints = BitConverter.ToUInt16(frame, 4);
		}
	}
}
