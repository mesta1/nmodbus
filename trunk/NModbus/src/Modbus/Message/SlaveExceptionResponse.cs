using System;
using System.Collections.Generic;
using System.Text;

namespace Modbus.Message
{
	public class SlaveExceptionResponse : ModbusMessage, IModbusMessage
	{
		private const int MinFrameSize = 3;

		public SlaveExceptionResponse()
		{
		}

		// TODO do we want to manipulate functionCode?
		public SlaveExceptionResponse(byte slaveAddress, byte functionCode, byte exceptionCode)
			: base(slaveAddress, (byte) (functionCode + Modbus.ExceptionOffset))
		{
			SlaveExceptionCode = exceptionCode;
		}

		public byte SlaveExceptionCode
		{
			get { return MessageImpl.ExceptionCode; }
			set { MessageImpl.ExceptionCode = value; }
		}

		protected override void InitializeUnique(byte[] frame)
		{
			if (frame.Length < MinFrameSize)
				throw new FormatException(String.Format("Message frame must contain at least {0} bytes of data.", MinFrameSize));

			SlaveExceptionCode = frame[2];	
		}
	}
}
