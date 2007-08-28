using System;
using System.Net;

namespace Modbus.Message
{
	class ReadHoldingInputRegistersRequest : ModbusMessage, IModbusMessage
	{
		private const int _minimumFrameSize = 6;

		public ReadHoldingInputRegistersRequest()
		{
		}

		public ReadHoldingInputRegistersRequest(byte functionCode, byte slaveAddress, ushort startAddress, ushort numberOfPoints)
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
				if (value > Modbus.MaximumRegisterRequestResponseSize)
					throw new ArgumentOutOfRangeException("NumberOfPoints", String.Format("Maximum amount of data {0} registers.", Modbus.MaximumRegisterRequestResponseSize));

				MessageImpl.NumberOfPoints = value;
			}
		}

		public override string ToString()
		{
			return String.Format("Read {0} {1} registers at address {2}.", NumberOfPoints, FunctionCode == Modbus.ReadHoldingRegisters ? "holding" : "input", StartAddress);
		}

		protected override void InitializeUnique(byte[] frame)
		{
			StartAddress = (ushort) IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 2));
			NumberOfPoints = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 4));
		}
	}
}
