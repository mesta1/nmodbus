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

		// TODO do we want to manipulate functionCode?
		public SlaveExceptionResponse(byte slaveAddress, byte functionCode, byte exceptionCode)
			: base(slaveAddress, (byte) (functionCode + Modbus.ExceptionOffset))
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
			SlaveExceptionCode = frame[2];	
		}
	}
}
