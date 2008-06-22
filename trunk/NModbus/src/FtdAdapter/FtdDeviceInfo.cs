namespace FtdAdapter
{
	/// <summary>
	/// Provides information about an attached FTDI USB device.
	/// </summary>
	public struct FtdDeviceInfo
	{
		private readonly uint _flags;
		private readonly uint _id;
		private readonly string _serialNumber;
		private readonly uint _type;
		private readonly uint _locationId;
		private readonly string _description;

		internal FtdDeviceInfo(uint flags, uint type, uint id, uint locId, string serialNumber, string description)
		{
			_flags = flags;
			_type = type;
			_id = id;
			_locationId = locId;
			_serialNumber = serialNumber;
			_description = description;
		}
		
		/// <summary>
		/// Complete device ID, comprising Vendor ID and Product ID.
		/// </summary>
		public uint Id
		{
			get { return _id; }
		}

		/// <summary>
		/// Vendor ID.
		/// </summary>
		public uint VendorId
		{
			get { return (_id >> 16) & 0xFFFF; }
		}

		/// <summary>
		/// Product ID
		/// </summary>
		public uint ProductId
		{
			get { return _id & 0xFFFF; }
		}	

		/// <summary>
		/// Serial number of device.
		/// </summary>
		public string SerialNumber
		{
			get { return _serialNumber; }
		}

		/// <summary>
		/// Device flags.
		/// </summary>
		public uint Flags
		{
			get { return _flags; }
		}

		/// <summary>
		/// Device type.
		/// </summary>
		public uint Type
		{
			get { return _type; }
		}

		/// <summary>
		/// LocID
		/// </summary>
		public uint LocationId
		{
			get { return _locationId; }
		}

		/// <summary>
		/// Description of device.
		/// </summary>
		public string Description
		{
			get { return _description; }
		}

		/// <summary>
		/// Gets a value indicating if the device is already open.
		/// </summary>
		public bool IsOpen
		{
			get { return (_flags & 0x01) != 0; }
		}
	}
}
