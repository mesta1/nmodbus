using System;
using System.Collections.Generic;
using System.Text;

namespace Modbus
{	
	public static class Modbus
	{			
		// public function codes
		public const byte READ_COILS = 1;
		public const byte READ_INPUTS = 2;
		public const byte READ_HOLDING_REGISTERS = 3;
		public const byte READ_INPUT_REGISTERS = 4;
		public const byte WRITE_SINGLE_COIL = 5;
		public const byte WRITE_SINGLE_REGISTER = 6;
		public const byte READ_EXCEPTION_STATUS = 7;
		public const byte DIAGNOSTICS = 8;


		public const byte WRITE_MULTIPLE_REGISTERS = 16;

		
		// standard function code
		public const int WRITE_MULTIPLE_COILS = 15;
				
		// byte representation of the coil state <b>on</b>.		
		public const int COIL_ON = (byte) 255;

		// byte representation of the coil state <b>pos</b>.
		public const int COIL_OFF = 0;

		// maximum number of bits in multiple read/write of input discretes or coils (<b>2000</b>).
		public const int MAX_BITS = 2000;

		// modbus slave exception offset that is added to the function code, to flag an exception.
		// the last valid function code is 127
		public const byte EXCEPTION_OFFSET = 128;

		/**
		* Defines the Modbus slave exception type <tt>illegal function</tt>.
		* This exception code is returned if the slave:
		* <ul>
		*   <li>does not implement the function code <b>or</b></li>
		*   <li>is not in a state that allows it to process the function</li>
		* </ul>
		*/
		public const int ILLEGAL_FUNCTION_EXCEPTION = 1;

		/**
		* Defines the Modbus slave exception type <tt>illegal data address</tt>.
		* This exception code is returned if the reference:
		* <ul>
		*   <li>does not exist on the slave <b>or</b></li>
		*   <li>the combination of reference and length exceeds the bounds
		*       of the existing registers.
		*   </li>
		* </ul>
		*/
		public const int ILLEGAL_ADDRESS_EXCEPTION = 2;

		/**
		* Defines the Modbus slave exception type <tt>illegal data value</tt>.
		* This exception code indicates a fault in the structure of the data values
		* of a complex request, such as an incorrect implied length.<br>
		* <b>This code does not indicate a problem with application specific validity
		* of the value.</b>
		*/
		public const int ILLEGAL_VALUE_EXCEPTION = 3;

		/**
		* Defines the default port number of Modbus
		* (=<tt>502</tt>).
		*/
		public const int DEFAULT_PORT = 502;

		/**
		* Defines the maximum message length in bytes
		* (=<tt>256</tt>).
		*/
		public const int MAX_MESSAGE_LENGTH = 256;

		/**
		* Defines the default transaction identifier (=<tt>0</tt>).
		*/
		public const ushort DEFAULT_TRANSACTION_ID = 0;

		/**
		* Defines the default protocol identifier (=<tt>0</tt>).
		*/
		public const int DEFAULT_PROTOCOL_ID = 0;

		/**
		* Defines the default setting for validity checking
		* in transactions (=<tt>true</tt>).
		*/
		public const bool DEFAULT_CHECKVALIDITY = true;

		/**
		* Defines the default setting for I/O operation timeouts
		* in milliseconds (=<tt>3000</tt>).
		*/
		public const int DEFAULT_TIMEOUT = 3000;

		/**
		* Defines the default reconnecting setting for
		* transactions (=<tt>false</tt>).
		*/
		public const bool DEFAULT_RECONNECTING = false;

		/**
		* Defines the default amount of retires for opening
		* a connection (=<tt>3</tt>).
		*/
		public const int DEFAULT_RETRIES = 3;

		/**
		* Defines the default number of msec to delay before transmission
		* (=<tt>50</tt>).
		*/
		public const int DEFAULT_TRANSMIT_DELAY = 0;

		/**
		* Defines the maximum value of the transaction identifier.
		*/
		//public const int MAX_TRANSACTION_ID = (Short.MAX_VALUE * 2);


		/**
		* Defines the serial encoding "ASCII".
		*/
		public const string SERIAL_ENCODING_ASCII = "ascii";

		/**
		* Defines the serial encoding "RTU".
		*/
		public const string SERIAL_ENCODING_RTU = "rtu";

		/**
		* Defines the serial encoding "BIN".
		*/
		public const string SERIAL_ENCODING_BIN = "bin";

		/**
		* Defines the default serial encoding (ASCII).
		*/
		public const string DEFAULT_SERIAL_ENCODING = SERIAL_ENCODING_ASCII;
	}
}
