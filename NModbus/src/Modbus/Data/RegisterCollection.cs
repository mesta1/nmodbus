using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Modbus.Util;
using System.Net;

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

		public static RegisterCollection CreateRegisterCollection(ushort defaultValue, int size)
		{
			if (size < 0)
				throw new ArgumentException("RegisterCollection size cannot be less than 0.");

			RegisterCollection col = new RegisterCollection();

			for (int i = 0; i < size; i++)
				col.Add(defaultValue);

			return col;
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
