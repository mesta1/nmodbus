using System;
using System.Collections.Generic;
using System.Text;

namespace Modbus
{
	public class SlaveException : Exception
	{
		private byte _slaveExceptionCode;

		public SlaveException(byte slaveExceptionCode) 
		{
			_slaveExceptionCode = slaveExceptionCode;
		}

		public SlaveException(byte slaveExceptionCode, string message)
			: base(message)
		{
			_slaveExceptionCode = slaveExceptionCode;
		}

		public SlaveException(byte slaveExceptionCode, string message, Exception innerException)
			: base(message, innerException)
		{
			_slaveExceptionCode = slaveExceptionCode;
		}

		public byte SlaveExceptionCode
		{
			get { return _slaveExceptionCode; }
		}	
	}
}
