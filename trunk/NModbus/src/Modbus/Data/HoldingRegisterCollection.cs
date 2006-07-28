using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Modbus.Util;

namespace Modbus.Data
{
	public class HoldingRegisterCollection : Collection<ushort>, RegisterMixin<HoldingRegisterCollection>.IMixin
	{
		RegisterMixin<HoldingRegisterCollection> _mixin;

		public HoldingRegisterCollection()
		{
		}

		public HoldingRegisterCollection(params ushort[] registers)
			: this((IList<ushort>) registers)
		{
		}

		public HoldingRegisterCollection(byte[] bytes)
			: this((IList<ushort>) ModbusUtil.NetworkBytesToHostUInt16(bytes))
		{
		}

		public HoldingRegisterCollection(IList<ushort> registers)
			: base(registers.IsReadOnly ? new List<ushort>(registers) : registers)
		{
		}
		
		public byte[] Bytes
		{
			get
			{
				return _mixin.GetBytes(this);
			}
		}	
	}
}
