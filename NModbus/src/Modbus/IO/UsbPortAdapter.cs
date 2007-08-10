namespace Modbus.IO
{
	internal class UsbPortAdapter : ISerialResource
	{
		private readonly FtdUsbPort _usbPort;

		public UsbPortAdapter(FtdUsbPort usbPort)
		{
			_usbPort = usbPort;
		}

		public string NewLine
		{
			get { return _usbPort.NewLine; }
			set { _usbPort.NewLine = value; }
		}

		public int ReadTimeout
		{
			get { return _usbPort.ReadTimeout; }
			set { _usbPort.ReadTimeout = value; }
		}

		public int WriteTimeout
		{
			get { return _usbPort.ReadTimeout; }
			set { _usbPort.ReadTimeout = value; }
		}

		public void DiscardInBuffer()
		{
			_usbPort.PurgeReceiveBuffer();
		}

		public int Read(byte[] buffer, int offset, int count)
		{
			return _usbPort.Read(buffer, offset, count);
		}

		public string ReadLine()
		{
			return _usbPort.ReadLine();
		}

		public void Write(byte[] buffer, int offset, int count)
		{
			_usbPort.Write(buffer, offset, count);
		}
	}
}
