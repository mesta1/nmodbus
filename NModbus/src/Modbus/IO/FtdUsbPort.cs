using System;
using System.IO;
using System.Runtime.InteropServices;
using Modbus.Util;
using System.Text;
using System.Collections.Generic;

namespace Modbus.IO
{
	/// <summary>
	/// Specifies the number of stop bits used on the UsbPort object.
	/// </summary>
	public enum FtdStopBits
	{
		/// <summary>
		/// One stop bit is used.
		/// </summary>
		One = 1,
		/// <summary>
		/// 1.5 stop bits are used.
		/// </summary>
		OnePointFive,
		/// <summary>
		/// Two stop bits are used.
		/// </summary>
		Two
	}

	/// <summary>
	/// Specifies the parity used on the UsbPort object.
	/// </summary>
	public enum FtdParity
	{
		/// <summary>
		/// No parity check occurs.
		/// </summary>
		None = 0,
		/// <summary>
		/// Sets the parity bit so that the count of bits set is an odd number.
		/// </summary>
		Odd,
		/// <summary>
		/// Sets the parity bit so that the count of bits set is an even number.
		/// </summary>
		Even,
		/// <summary>
		/// Leaves the parity bit set to 1.
		/// </summary>
		Mark,
		/// <summary>
		/// Leaves the parity bit set to 0.
		/// </summary>
		Space
	}

	/// <summary>
	/// Specifies the result of a UsbPort operation.
	/// </summary>
	internal enum FtdStatus
	{
		OK = 0,
		InvalidHandle,
		DeviceNotFound,
		DeviceNotOpened,
		IOError,
		InsufficientResources,
		InvalidParameter,
		InvalidBaudRate,
		DeviceNotOpenedForErase,
		DeviceNotOpenedForWrite,
		FailedToWriteDevice,
		EEPromReadFailed,
		EEPromWriteFailed,
		EEPromEraseFailed,
		EEPromNotPresent,
		EEPromNotProgrammed,
		InvalidArgs,
		OtherError
	};

	/// <summary>
	/// Wrapper class for the FTD2XX USB resource.
	/// </summary>
	public class FtdUsbPort : IDisposable
	{
		internal const string FtdAssemblyName = "FTD2XX.dll";

		[DllImport(FtdAssemblyName)]
		static extern FtdStatus FT_Close(uint deviceHandle);
		[DllImport(FtdAssemblyName)]
		static extern FtdStatus FT_Open(uint deviceID, ref uint deviceHandle);
		[DllImport(FtdAssemblyName)]
		static extern FtdStatus FT_SetBaudRate(uint deviceHandle, uint baudRate);
		[DllImport(FtdAssemblyName)]
		static extern FtdStatus FT_SetDataCharacteristics(uint deviceHandle, byte wordLength, byte stopBits, byte parity);
		[DllImport(FtdAssemblyName)]
		static extern unsafe FtdStatus FT_Read(uint deviceHandle, void* buffer, uint bytesToRead, ref uint bytesReturned);
		[DllImport(FtdAssemblyName)]
		static extern unsafe FtdStatus FT_Write(uint deviceHandle, void* buffer, uint bytesToWrite, ref uint bytesWritten);
		[DllImport(FtdAssemblyName)]
		static extern FtdStatus FT_SetTimeouts(uint deviceHandle, uint readTimeout, uint writeTimeout);
		[DllImport(FtdAssemblyName)]
		static extern FtdStatus FT_Purge(uint deviceHandle, uint mask);
		[DllImport(FtdAssemblyName)]
		static extern FtdStatus FT_CreateDeviceInfoList(ref uint deviceCount);

		private const byte PurgeRx = 1;
		private uint _deviceID;
		private string _newLine = Environment.NewLine;
		private uint _deviceHandle;
		private uint _readTimeout = Modbus.DefaultTimeout;
		private uint _writeTimeout = Modbus.DefaultTimeout;
		private int _baudRate = 9600;
		private int _dataBits = 8;
		private byte _stopBits = 1;
		private byte _parity = 0;

		/// <summary>
		/// Initializes a new instance of the <see cref="FtdUsbPort"/> class.
		/// </summary>
		public FtdUsbPort()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FtdUsbPort"/> class.
		/// </summary>
		/// <param name="deviceID">The device ID.</param>
		public FtdUsbPort(uint deviceID)
		{
			_deviceID = deviceID;
		}

		/// <summary>
		/// Gets or sets the device ID.
		/// </summary>
		public int DeviceID
		{
			get
			{
				return (int) _deviceID;
			}
			set
			{
				if (value < 0)
					throw new ArgumentException("Device ID must be greater than 0.");

				if (IsOpen)
					throw new InvalidOperationException("Cannot set the Device ID when the port is open");

				_deviceID = (uint) value;
			}
		}

		/// <summary>
		/// Gets or sets the value used to interpret the end of a call to the ReadLine method. 
		/// </summary>
		public string NewLine
		{
			get { return _newLine; }
			set { _newLine = value; }
		}

		/// <summary>
		/// Gets or sets the serial baud rate. 
		/// </summary>
		public int BaudRate
		{
			get
			{
				return _baudRate;
			}
			set
			{
				if (value <= 0)
					throw new ArgumentOutOfRangeException("BaudRate", "BaudRate must be greater than 0.");

				_baudRate = value;

				if (IsOpen)
					InvokeFtdMethod(delegate { return FT_SetBaudRate(_deviceHandle, (uint) _baudRate); });
			}
		}

		/// <summary>
		/// Gets or sets the standard length of data bits per byte. 
		/// </summary>
		public int DataBits
		{
			get
			{
				return _dataBits;
			}
			set
			{
				if (value < 5 || value > 8)
					throw new ArgumentOutOfRangeException("DataBits", "Value must be greater than 4 and less than 9.");

				_dataBits = value;

				if (IsOpen)
					InvokeFtdMethod(delegate { return FT_SetDataCharacteristics(_deviceHandle, (byte) _dataBits, _stopBits, _parity); });
			}
		}

		/// <summary>
		/// Gets or sets the number of milliseconds before a time-out occurs when a read operation does not finish.
		/// </summary>
		public int ReadTimeout
		{
			get
			{
				return (int) _readTimeout;
			}
			set
			{
				if (value < 0)
					throw new ArgumentOutOfRangeException("ReadTimeout", "Read timeout must be greater than 0.");

				_readTimeout = (uint) value;

				if (IsOpen)
					InvokeFtdMethod(delegate { return FT_SetTimeouts(_deviceHandle, _readTimeout, _writeTimeout); });
			}
		}

		/// <summary>
		/// Gets or sets the number of milliseconds before a time-out occurs when a write operation does not finish.
		/// </summary>
		public int WriteTimeout
		{
			get
			{
				return (int) _writeTimeout;
			}
			set
			{
				if (value < 0)
					throw new ArgumentOutOfRangeException("WriteTimeout", "Write timeout must be greater than 0.");

				_writeTimeout = (uint) value;

				if (IsOpen)
					InvokeFtdMethod(delegate { return FT_SetTimeouts(_deviceHandle, _readTimeout, _writeTimeout); });
			}
		}

		/// <summary>
		/// Gets or sets the standard number of stopbits per byte.
		/// </summary>
		public FtdStopBits StopBits
		{
			get
			{
				return (FtdStopBits) Enum.Parse(typeof(FtdStopBits), _stopBits.ToString());
			}
			set
			{
				_stopBits = (byte) value;

				if (IsOpen)
					InvokeFtdMethod(delegate { return FT_SetDataCharacteristics(_deviceHandle, (byte) _dataBits, _stopBits, _parity); });
			}
		}

		/// <summary>
		/// Gets or sets the parity-checking protocol.
		/// </summary>
		public FtdParity Parity
		{
			get
			{
				return (FtdParity) Enum.Parse(typeof(FtdParity), _parity.ToString());
			}
			set
			{
				_parity = (byte) value;

				if (IsOpen)
					InvokeFtdMethod(delegate { return FT_SetDataCharacteristics(_deviceHandle, (byte) _dataBits, _stopBits, _parity); });
			}
		}

		/// <summary>
		/// Gets a value indicating the open or closed status of the UsbPort object.
		/// </summary>
		public bool IsOpen
		{
			get
			{
				return _deviceHandle != 0;
			}
		}

		internal uint DeviceHandle
		{
			get { return _deviceHandle; }
			set { _deviceHandle = value; }
		}

		/// <summary>
		/// Returns the number of D2XX devices connected to the system.
		/// </summary>
		public static int DeviceCount()
		{
			uint deviceCount = 0;
			InvokeFtdMethod(delegate { return FT_CreateDeviceInfoList(ref deviceCount); });

			return (int) deviceCount;
		}

		/// <summary>
		/// Opens a new port connection.
		/// </summary>
		public void Open()
		{
			InvokeFtdMethod(delegate { return FT_Open(_deviceID, ref _deviceHandle); });
			BaudRate = _baudRate;
			InvokeFtdMethod(delegate { return FT_SetDataCharacteristics(_deviceHandle, (byte) _dataBits, _stopBits, _parity); });
			InvokeFtdMethod(delegate { return FT_SetTimeouts(_deviceHandle, _readTimeout, _writeTimeout); });
		}

		/// <summary>
		/// Closes the port connection.
		/// </summary>
		public void Close()
		{
			try
			{
				InvokeFtdMethod(delegate { return FT_Close(_deviceHandle); });
			}
			finally
			{
				_deviceHandle = 0;
			}
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			if (IsOpen)
				Close();
		}

		/// <summary>
		/// Writes a specified number of bytes to the port from an output buffer, starting at the specified offset.
		/// </summary>
		/// <param name="buffer">The byte array that contains the data to write to the port.</param>
		/// <param name="offset">The offset in the buffer array to begin writing.</param>
		/// <param name="size">The number of bytes to write.</param>
		public unsafe void Write(byte[] buffer, int offset, int size)
		{
			uint numBytesReturned = 0;

			fixed (byte* pBuf = buffer)
			{
				InvokeFtdMethod(delegate { return FT_Write(_deviceHandle, pBuf, (uint) size, ref numBytesReturned); });
			}
		}

		/// <summary>
		/// Reads up to the NewLine value in the input buffer. 
		/// </summary>
		public string ReadLine()
		{
			StringBuilder result = new StringBuilder();
			byte[] singleByteBuffer = new byte[1];

			do
			{
				if (Read(singleByteBuffer, 0, 1) == 0)
					throw new IOException("0 read in input buffer before NewLine encountered.");

				result.Append(Encoding.ASCII.GetChars(singleByteBuffer)[0]);

			} while (!result.ToString().EndsWith(NewLine));

			return result.ToString().Substring(0, result.Length - NewLine.Length);
		}

		/// <summary>
		/// Reads a number of bytes from the UsbPort input buffer and writes those bytes into a byte array at the specified offset.
		/// </summary>
		/// <param name="buffer">The byte array to write the input to.</param>
		/// <param name="offset">The offset in the buffer array to begin writing.</param>
		/// <param name="size">The number of bytes to read.</param>
		/// <returns>The number of bytes read.</returns>
		public unsafe int Read(byte[] buffer, int offset, int size)
		{
			// TODO implement offset
			uint numBytesReturned = 0;

			fixed (byte* pBuf = buffer)
			{
				InvokeFtdMethod(delegate { return FT_Read(_deviceHandle, pBuf, (uint) size, ref numBytesReturned); });
			}

			return (int) numBytesReturned;
		}

		/// <summary>
		/// Purges the receive buffer.
		/// </summary>
		public void PurgeReceiveBuffer()
		{
			InvokeFtdMethod(delegate { return FT_Purge(_deviceHandle, PurgeRx); });
		}

		/// <summary>
		/// Invokes FT method and checks the FTStatus result, throw IOException if result is something other than FTStatus.OK
		/// </summary>
		internal static void InvokeFtdMethod(Func<FtdStatus> func)
		{
			FtdStatus status = func();

			if (status != FtdStatus.OK)
			{
				throw new IOException(Enum.GetName(typeof(FtdStatus), status));
			}
		}
	}
}
