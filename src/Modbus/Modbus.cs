namespace Modbus
{	
	/// <summary>
	///  Defines constants related to the Modbus protocol.
	/// </summary>
	internal static class Modbus
	{			
		// supported function codes
		public const byte ReadCoils = 1;
		public const byte ReadInputs = 2;
		public const byte ReadHoldingRegisters = 3;
		public const byte ReadInputRegisters = 4;
		public const byte WriteSingleCoil = 5;
		public const byte WriteSingleRegister = 6;		
		public const byte Diagnostics = 8;
		public const ushort DiagnosticsReturnQueryData = 0;
		public const byte WriteMultipleCoils = 15;
		public const byte WriteMultipleRegisters = 16;
		public const byte ReadWriteMultipleRegisters = 23;

		public const int MaximumDiscreteRequestResponseSize = 2040;
		public const int MaximumRegisterRequestResponseSize = 127; 

		// modbus slave exception offset that is added to the function code, to flag an exception
		public const byte ExceptionOffset = 128;

		// modbus slave exception codes
		public const byte Acknowlege = 5;

		// default setting for number of retries for IO operations
		public const int DefaultRetries = 3;

		// default setting for IO timeouts in milliseconds
		public const int DefaultTimeout = 1000;

		// smallest supported message frame size (sans checksum)
		public const int MinimumFrameSize = 2;

		public const ushort CoilOn = 0xFF00;
		public const ushort CoilOff = 0x0000;
	
		// IP slaves should be addressed by IP
		public const byte DefaultIpSlaveUnitId = 0;

		// An established connection was aborted by the software in your host computer, possibly due to a data transmission time-out or protocol error
		public const int ConnectionAborted = 10053;

		// An existing connection was forcibly closed by the remote host
		public const int ConnectionResetByPeer = 10054;
	}
}
