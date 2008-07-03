using System;
using System.Collections.Generic;
using System.Text;

namespace Modbus.IO
{
	/// <summary>
	/// Adapter so we can mock the serial port
	/// </summary>
	public interface ISerialPortAdapter
	{
		string ReadLine();

		int Read(byte[] buffer, int offset, int count);
		
		void Write(byte[] buffer, int offset, int count);
	}
}
