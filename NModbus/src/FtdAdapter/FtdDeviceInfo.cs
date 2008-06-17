using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FtdAdapter
{
	/// <summary>
	/// Provides information about attached FTDI USB devices.
	/// </summary>
	public struct FtdDeviceInfo
	{
		internal FtdDeviceInfo(uint flags, uint type, uint id, uint locId, string serialNumber, string description)
		{
			this.flags = flags;
			this.type = type;
			this.id = id;
			this.locId = locId;
			this.serialNumber = serialNumber;
			this.description = description;
		}

		private readonly uint id;

		/// <summary>
		/// Complete device ID, comprising Vid and Pid
		/// </summary>
		public uint Id
		{
			get { return id; }
		}

		/// <summary>
		/// Vendor ID
		/// </summary>
		public uint Vid
		{
			get { return (id >> 16) & 0xFFFF; }
		}

		/// <summary>
		/// Product ID
		/// </summary>
		public uint Pid
		{
			get { return id & 0xFFFF; }
		}

		private readonly string serialNumber;

		/// <summary>
		/// Serial number of device.
		/// </summary>
		public string SerialNumber
		{
			get { return serialNumber; }
		}

		private uint flags;

		/// <summary>
		/// Device flags
		/// </summary>
		public uint Flags
		{
			get { return flags; }
		}

		private uint type;

		/// <summary>
		/// Device type.
		/// </summary>
		public uint Type
		{
			get { return type; }
		}

		private uint locId;

		/// <summary>
		/// LocID
		/// </summary>
		public uint Locid
		{
			get { return locId; }
		}

		private string description;

		/// <summary>
		/// Description of device.
		/// </summary>
		public string Description
		{
			get { return description; }
		}

		/// <summary>
		/// Gets a value indicating if the device is already open.
		/// </summary>
		public bool IsOpen
		{
			get { return (flags & 0x01) != 0; }
		}

		/// <summary>
		/// String representation of the device.
		/// </summary>
		/// <returns>Returns the device description.</returns>
		public override string ToString()
		{
			return description;
		}
	}
}
