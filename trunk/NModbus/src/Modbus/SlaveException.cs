using System;
using Modbus.Message;
using System.Runtime.Serialization;

namespace Modbus
{
	/// <summary>
	/// Represents slave errors that occur during communication.
	/// </summary>
	[Serializable]
	public class SlaveException : Exception
	{
		private readonly SlaveExceptionResponse _slaveExceptionResponse;

		/// <summary>
		/// Initializes a new instance of the <see cref="SlaveException"/> class.
		/// </summary>
		public SlaveException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SlaveException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public SlaveException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SlaveException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The inner exception.</param>
		public SlaveException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SlaveException"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
		/// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
		protected SlaveException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			// TODO: Implement type-specific serialization constructor logic.
		}

		internal SlaveException(SlaveExceptionResponse slaveExceptionResponse)
		{
			_slaveExceptionResponse = slaveExceptionResponse;
		}

		internal SlaveException(string message, SlaveExceptionResponse slaveExceptionResponse)
			: base(message)
		{
			_slaveExceptionResponse = slaveExceptionResponse;
		}

		/// <summary>
		/// Gets a message that describes the current exception.
		/// </summary>
		/// <value></value>
		/// <returns>The error message that explains the reason for the exception, or an empty string("").</returns>
		public override string Message
		{
			get
			{
				return String.Concat(base.Message, _slaveExceptionResponse != null ? String.Concat(Environment.NewLine, _slaveExceptionResponse) : String.Empty);
			}
		}

		/// <summary>
		/// Gets the response function code that caused the exception to occur, or 0.
		/// </summary>
		/// <value>The function code.</value>
		public byte FunctionCode
		{
			get
			{
				return _slaveExceptionResponse != null ? _slaveExceptionResponse.FunctionCode : (byte) 0;
			}
		}

		/// <summary>
		/// Gets the slave exception code, or 0.
		/// </summary>
		/// <value>The slave exception code.</value>
		public byte SlaveExceptionCode
		{
			get
			{
				return _slaveExceptionResponse != null ? _slaveExceptionResponse.SlaveExceptionCode : (byte) 0;
			}
		}
	}
}
