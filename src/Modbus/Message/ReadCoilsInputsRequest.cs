using System;
using System.Net;

namespace Modbus.Message
{
	class ReadCoilsInputsRequest : ModbusMessage, IModbusMessage
	{
		private const int _minimumFrameSize = 6;

		public ReadCoilsInputsRequest()
		{
		}

		public ReadCoilsInputsRequest(byte functionCode, byte slaveAddress, ushort startAddress, ushort numberOfPoints)
			: base(slaveAddress, functionCode)
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

		public override string ToString()
		{
			return String.Format("Read {0} {1} beginning at address {2}.", NumberOfPoints, FunctionCode == Modbus.ReadCoils ? "coils" : "inputs", StartAddress);
		}

		protected override void InitializeUnique(byte[] frame)
		{
			StartAddress = (ushort) IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 2));
			NumberOfPoints = (ushort) IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 4));
		}
	}
}
