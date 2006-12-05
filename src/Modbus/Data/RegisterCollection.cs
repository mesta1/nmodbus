using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Modbus.Util;
using System.Net;
using System.Collections;

namespace Modbus.Data
{
	public class RegisterCollection : Collection<ushort>, IModbusMessageDataCollection
	{
		public RegisterCollection()
		{
		}

		public RegisterCollection(byte[] bytes)
			: this((IList<ushort>) ModbusUtil.NetworkBytesToHostUInt16(bytes))
		{
		}

		public RegisterCollection(params ushort[] registers)
			: this((IList<ushort>) registers)
		{
		}

		public RegisterCollection(IList<ushort> registers)
			: base(registers.IsReadOnly ? new List<ushort>(registers) : registers)
		{
		}

		public byte[] NetworkBytes
		{
			get
			{
				List<byte> bytes = new List<byte>();

				foreach (ushort register in this)
					bytes.AddRange(BitConverter.GetBytes((ushort) IPAddress.HostToNetworkOrder((short) register)));

				return bytes.ToArray();
			}
		}

		public byte ByteCount
		{
			get 
			{ 
				return (byte) (Count * 2);
			}
		}
	}
}
