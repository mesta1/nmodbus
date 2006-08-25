using System;
using System.Collections.Generic;
using System.Text;

namespace Modbus.Message
{
	public class SlaveExceptionResponse : ModbusMessage, IModbusMessage
	{
		private const int _minimumFrameSize = 3;

		public SlaveExceptionResponse()
		{
		}

		public SlaveExceptionResponse(byte slaveAddress, byte functionCode, byte exceptionCode)
			: base(slaveAddress, functionCode)
		{
			SlaveExceptionCode = exceptionCode;
		}

		public override int MinimumFrameSize
		{
			get { return _minimumFrameSize; }
		}

		public byte SlaveExceptionCode
		{
			get { return MessageImpl.ExceptionCode; }
			set { MessageImpl.ExceptionCode = value; }
		}

		protected override void InitializeUnique(byte[] frame)
		{
			if (FunctionCode <= Modbus.ExceptionOffset)
				throw new FormatException("Invalid function code value for SlaveExceptionResponse.");

			FunctionCode -= Modbus.ExceptionOffset;
			SlaveExceptionCode = frame[2];
		}
	}
}