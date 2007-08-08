namespace Modbus.IO
{
	interface ISerialResource
	{
		int ReadTimeout { get; set; }
		int WriteTimeout { get; set; }
		string NewLine { get; set; }
		void DiscardInBuffer();
		int Read(byte[] buffer, int offset, int count);
		string ReadLine();
		void Write(byte[] buffer, int offset, int count);
	}
}
