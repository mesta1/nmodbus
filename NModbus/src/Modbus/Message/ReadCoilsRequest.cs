using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Data;

namespace Modbus.Message
{
	public class ReadCoilsRequest : ModbusMessage, IModbusMessage
	{
		private const int MinFrameSize = 6;

		public ReadCoilsRequest()
		{
		}

		public ReadCoilsRequest(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
			: base(slaveAddress, Modbus.READ_COILS)
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
			if (frame.Length < 6)
				throw new FormatException(String.Format("Message frame must contain at least {0} bytes of data.", MinFrameSize));

			StartAddress = BitConverter.ToUInt16(frame, 2);
			NumberOfPoints = BitConverter.ToUInt16(frame, 4);
		}
	}
}
