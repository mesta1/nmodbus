using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Message;
using System.Reflection;

namespace Modbus
{
	public class SlaveException : Exception
	{
		private SlaveExceptionResponse _slaveExceptionResponse;

		public SlaveException()
		{
		}

		public SlaveException(SlaveExceptionResponse slaveExceptionResponse) 
		{
			_slaveExceptionResponse = slaveExceptionResponse;
		}

		public SlaveException(string message, SlaveExceptionResponse slaveExceptionResponse)
			: base(message)
		{
			_slaveExceptionResponse = slaveExceptionResponse;
		}

		public SlaveException(string message, Exception innerException, SlaveExceptionResponse slaveExceptionResponse)
			: base(message, innerException)
		{
			_slaveExceptionResponse = slaveExceptionResponse;
		}

		public override string Message
		{
			get
			{
				if (_slaveExceptionResponse == null)
					return base.Message;

				return String.Format("{0}{1}Function Code: {2}{1}Exception Code: {3}", base.Message, Environment.NewLine, _slaveExceptionResponse.FunctionCode, _slaveExceptionResponse.SlaveExceptionCode);
			}
		}
	}
}
