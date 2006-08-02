using System;
using System.Collections.Generic;
using System.Text;

namespace Modbus.Message
{
	public class ReadInputsRequest : ModbusMessage, IModbusMessage
	{
		private const int _minimumFrameSize = 6;

		public ReadInputsRequest()
		{
		}

		public ReadInputsRequest(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
			: base(slaveAddress, Modbus.ReadInputs)
		{
			StartAddress = startAddress;
			NumberOfPoints = numberOfPoints;
		}

		public ushort StartAddress
		{
			get { return MessageImpl.StartAddress; }
			set { MessageImpl.StartAddress = value; }
		}

		public override int MinimumFrameSize
		{
			get { return _minimumFrameSize; }
		}

		public ushort NumberOfPoints
		{
			get { return MessageImpl.NumberOfPoints; }
			set { MessageImpl.NumberOfPoints = value; }
		}

		protected override void InitializeUnique(byte[] frame)
		{
			StartAddress = BitConverter.ToUInt16(frame, 2);
			NumberOfPoints = BitConverter.ToUInt16(frame, 4);
		}
	}
}
