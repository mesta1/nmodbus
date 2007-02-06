using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Data;

namespace Modbus.Message
{
	class ReadWriteMultipleRegistersRequest : ModbusMessage, IModbusMessage
	{
		private const int _minimumFrameSize = 11;

		public ReadWriteMultipleRegistersRequest()
		{
		}

		public ReadWriteMultipleRegistersRequest(ushort startReadAddress, ushort numberOfPointsToRead, ushort startWriteAddress, ushort numberOfPointsToWrite, RegisterCollection writeData)
			: base()
		{

		}

		public override int MinimumFrameSize
		{
			get { return MinimumFrameSize; }
		}

		protected override void InitializeUnique(byte[] frame)
		{
			if (frame.Length < _minimumFrameSize + frame[10])
				throw new FormatException("Message frame does not contain enough bytes.");



		}		
	}
}
