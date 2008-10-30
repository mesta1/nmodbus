using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Net;
using Modbus.Data;
using Unme.Common;

namespace Modbus.Message
{
	class WriteMultipleCoilsRequest : ModbusMessageWithData<DiscreteCollection>, IModbusMessage
	{
		private const int _minimumFrameSize = 7;

		public WriteMultipleCoilsRequest()
		{
		}

		public WriteMultipleCoilsRequest(byte slaveAddress, ushort startAddress, DiscreteCollection data)
			: base(slaveAddress, Modbus.WriteMultipleCoils)
		{
			StartAddress = startAddress;
			NumberOfPoints = (ushort) data.Count;
			ByteCount = (byte) ((data.Count + 7) / 8);
			Data = data;
		}

		public byte ByteCount
		{
			get { return MessageImpl.ByteCount; }
			set { MessageImpl.ByteCount = value; }
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
					throw new ArgumentOutOfRangeException("NumberOfPoints", String.Format(CultureInfo.InvariantCulture, "Maximum amount of data {0} coils.", Modbus.MaximumDiscreteRequestResponseSize));

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

		public override string ToString()
		{
			return String.Format(CultureInfo.InvariantCulture, "Write {0} coils starting at address {1}.", NumberOfPoints, StartAddress);
		}

		protected override void InitializeUnique(byte[] frame)
		{
			if (frame.Length < _minimumFrameSize + frame[6])
				throw new FormatException("Message frame does not contain enough bytes.");

			StartAddress = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 2));
			NumberOfPoints = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 4));
			ByteCount = frame[6];
			Data = new DiscreteCollection((new BitArray(frame.Slice(7, ByteCount).ToArray())).Cast<bool>().Take(NumberOfPoints).ToArray());
		}
	}
}
