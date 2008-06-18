using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Modbus.IO;
using Modbus.Utility;
using System.Globalization;
using System.Linq;

namespace FtdAdapter
{
	/// <summary>
	/// Wrapper class for the FTD2XX USB resource.
	/// </summary>
	public class FtdUsbPort : IStreamResource, IDisposable
	{
		[DllImport(FtdAssemblyName)]
		static extern FtdStatus FT_Close(uint deviceHandle);
		[DllImport(FtdAssemblyName)]
		static extern FtdStatus FT_Open(uint deviceId, ref uint deviceHandle);
		[DllImport(FtdAssemblyName)]
		static extern FtdStatus FT_SetBaudRate(uint deviceHandle, uint baudRate);
		[DllImport(FtdAssemblyName)]
		static extern FtdStatus FT_SetFlowControl(uint handle, ushort usFlowControl, byte uXon, byte uXoff);
		[DllImport(FtdAssemblyName)]
		static extern FtdStatus FT_SetDataCharacteristics(uint deviceHandle, byte wordLength, byte stopBits, byte parity);
		[DllImport(FtdAssemblyName)]
		static extern FtdStatus FT_Read(uint deviceHandle, byte[] buffer, uint bytesToRead, ref uint bytesReturned);
		[DllImport(FtdAssemblyName)]
		static extern FtdStatus FT_Write(uint deviceHandle, byte[] buffer, uint bytesToWrite, ref uint bytesWritten);
		[DllImport(FtdAssemblyName)]
		static extern FtdStatus FT_SetTimeouts(uint deviceHandle, uint readTimeout, uint writeTimeout);
		[DllImport(FtdAssemblyName)]
		static extern FtdStatus FT_Purge(uint deviceHandle, uint mask);
		[DllImport(FtdAssemblyName)]
		static extern FtdStatus FT_CreateDeviceInfoList(ref uint deviceCount);
		[DllImport(FtdAssemblyName)]
		static extern FtdStatus FT_GetDeviceInfoDetail(uint index, ref uint flags, ref uint type, ref uint id,
			ref uint locid, [In] [Out] byte[] serial, [In] [Out] byte[] description, ref uint deviceHandle);

		[DllImport(FtdAssemblyName)]
		static extern FtdStatus FT_OpenEx(byte[] arg1, uint flags, ref uint deviceHandle);

		private const string FtdAssemblyName = "FTD2XX.dll";
		private const byte PurgeRx = 1;
		private const uint _infiniteTimeout = 0;
		private readonly FtdDeviceInfo _deviceInfo;
		private uint _deviceId;
		private uint _deviceHandle;
		private uint _readTimeout = _infiniteTimeout;
		private uint _writeTimeout = _infiniteTimeout;
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
		/// <param name="deviceId">The device ID.</param>
		public FtdUsbPort(uint deviceId)
		{
			_deviceId = deviceId;
		}

		    /// <summary>
       /// Initializes a new instance of the <see cref="FtdUsbPort"/> class.
       /// </summary>
       /// <param name="deviceInfo">The device info.</param>
       public FtdUsbPort(FtdDeviceInfo deviceInfo)
       {
           _deviceInfo = deviceInfo;
       }

	   /// <summary>
	   /// Gets the device info.
	   /// </summary>
       public FtdDeviceInfo DeviceInfo
       {
           get { return _deviceInfo; }
       }

		/// <summary>
		/// Gets or sets the device ID.
		/// </summary>
		public int DeviceId
		{
			get
			{
				return (int) _deviceId;
			}
			set
			{
				if (value < 0)
					throw new ArgumentException("Device ID must be greater than 0.");

				if (IsOpen)
					throw new InvalidOperationException("Cannot set the Device ID when the port is open");

				_deviceId = (uint) value;
			}
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
		/// Indicates that no time-out should occur.
		/// </summary>
		public int InfiniteTimeout
		{
			get { return (int) _infiniteTimeout; }
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
				return (FtdStopBits) Enum.Parse(typeof(FtdStopBits), _stopBits.ToString(CultureInfo.InvariantCulture));
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
				return (FtdParity) Enum.Parse(typeof(FtdParity), _parity.ToString(CultureInfo.InvariantCulture));
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
			if (IsOpen)
				throw new InvalidOperationException("Port is already open.");

			if (_deviceInfo.SerialNumber == null)
			{
				InvokeFtdMethod(() => FT_Open(_deviceId, ref _deviceHandle));
			}
			else
			{
				var bytes = Encoding.ASCII.GetBytes(_deviceInfo.SerialNumber);
				InvokeFtdMethod(() => FT_OpenEx(bytes, (uint) OpenExFlags.BySerialNumber, ref _deviceHandle));
			}

			BaudRate = _baudRate;
			
			InvokeFtdMethod(() => FT_SetDataCharacteristics(_deviceHandle, (byte) _dataBits, _stopBits, _parity));
			InvokeFtdMethod(() => FT_SetTimeouts(_deviceHandle, _readTimeout, _writeTimeout));

			SetFlowControl(FtdFlowControl.None, 0, 0);
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
		/// <param name="count">The number of bytes to write.</param>
		public void Write(byte[] buffer, int offset, int count)
		{
			if (!IsOpen)
				throw new InvalidOperationException("Port not open.");
			if (buffer == null)
				throw new ArgumentNullException("buffer", "Argument buffer cannot be null.");
			if (offset < 0)
				throw new ArgumentOutOfRangeException("offset", "Argument offset must be greater than 0.");
			if (count < 0)
				throw new ArgumentOutOfRangeException("count", "Argument count must be greater than 0.");
			if ((buffer.Length - offset) < count)
				throw new ArgumentException("Invalid buffer size.");

			uint bytesWritten = 0;

			var buf = new byte[count];
			Array.Copy(buffer, offset, buf, 0, count);
			InvokeFtdMethod(() => FT_Write(_deviceHandle, buf, (uint) count, ref bytesWritten));

			if (count != 0 && bytesWritten == 0)
				throw new TimeoutException("The operation has timed out.");
			if (bytesWritten != count)
               throw new IOException("Not all bytes written to stream.");
		}

		/// <summary>
		/// Reads a number of bytes from the UsbPort input buffer and writes those bytes into a byte array at the specified offset.
		/// </summary>
		/// <param name="buffer">The byte array to write the input to.</param>
		/// <param name="offset">The offset in the buffer array to begin writing.</param>
		/// <param name="count">The number of bytes to read.</param>
		/// <returns>The number of bytes read.</returns>
		public int Read(byte[] buffer, int offset, int count)
		{
			if (!IsOpen)
				throw new InvalidOperationException("Port not open.");
			if (buffer == null)
				throw new ArgumentNullException("buffer", "Argument buffer cannot be null.");
			if (offset < 0)
				throw new ArgumentOutOfRangeException("offset", "Argument offset cannot be less than 0.");
			if (count < 0)
				throw new ArgumentOutOfRangeException("count", "Argument count cannot be less than 0.");
			if ((buffer.Length - offset) < count)
				throw new ArgumentException("Invalid buffer size.");

			uint numBytesReturned = 0;
			var buf = new byte[count];
			InvokeFtdMethod(() => FT_Read(_deviceHandle, buf, (uint) count, ref numBytesReturned));
			Array.Copy(buf, 0, buffer, offset, numBytesReturned);

			if (count != 0 && numBytesReturned == 0)
				throw new TimeoutException("The operation has timed out.");

			return (int) numBytesReturned;
		}

		/// <summary>
		/// Purges the receive buffer.
		/// </summary>
		public void DiscardInBuffer()
		{
			if (!IsOpen)
				throw new InvalidOperationException("Port is not open.");

			InvokeFtdMethod(delegate { return FT_Purge(_deviceHandle, PurgeRx); });
		}

		///<summary>
		/// Set flow control.
		///</summary>
		///<param name="FlowControl">Type of flow control</param>
		///<param name="Xon">XON symbol</param>
		///<param name="Xoff">XOFF symbol</param>
		public void SetFlowControl(FtdFlowControl FlowControl, byte Xon, byte Xoff)
		{
			InvokeFtdMethod(() => FT_SetFlowControl(_deviceHandle, (ushort) FlowControl, Xon, Xoff));
		}

		/// <summary>
		/// Gets the device info at the specified device index.
		/// </summary>
		/// <param name="index">Index of the device.</param>
		/// <returns>An FtdDeviceInfo instance.</returns>
		public static FtdDeviceInfo GetDeviceInfo(int index)
		{
			uint flags = 0;
			uint type = 0;
			uint id = 0;
			uint locid = 0;
			var serial = new byte[16];
			var description = new byte[64];
			uint handle = 0;

			InvokeFtdMethod(() => FT_GetDeviceInfoDetail((uint) index, ref flags, ref type, ref id, ref locid, serial, description, ref handle));

			return new FtdDeviceInfo(flags, type, id, locid, Encoding.ASCII.GetString(serial).Split('\0')[0], Encoding.ASCII.GetString(description).Split('\0')[0]);
		}

		/// <summary>
		/// Gets an array of all currently connected devices.
		/// </summary>
		/// <returns>An array of FtdDeviceInfo objects.</returns>
		public static FtdDeviceInfo[] GetDeviceInfos()
		{
			return Enumerable.Range(0, DeviceCount()).Select(n => GetDeviceInfo(n)).ToArray();
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
