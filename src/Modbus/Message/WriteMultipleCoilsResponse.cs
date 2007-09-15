using System;
using System.Net;

namespace Modbus.Message
{
	class WriteMultipleCoilsResponse : ModbusMessage, IModbusMessage
	{
		private const int _minimumFrameSize = 6;

		public WriteMultipleCoilsResponse()
		{
		}

		public WriteMultipleCoilsResponse(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
			: base(slaveAddress, Modbus.WriteMultipleCoils)
		{
			StartAddress = startAddress;
			NumberOfPoints = numberOfPoints;
		}

		public ushort NumberOfPoints
		{
			get
			{
				return MessageImpl.NumberOfPoints;
			}
			set
			{
				if (value > Modbus.MaximumDiscreteRequestResponseSize)
					throw new ArgumentOutOfRangeException("NumberOfPoints", String.Format("Maximum amount of data {0} coils.", Modbus.MaximumDiscreteRequestResponseSize));

				MessageImpl.NumberOfPoints = value;
			}
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

		protected override void InitializeUnique(byte[] frame)
		{
			StartAddress = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 2));
			NumberOfPoints = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 4));
		}
	}
}
