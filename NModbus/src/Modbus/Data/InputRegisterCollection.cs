using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Modbus.Data
{
	public class InputRegisterCollection : ReadOnlyCollection<ushort>, IModbusMessageDataCollection, RegisterMixin<InputRegisterCollection>.IMixin
	{
		RegisterMixin<InputRegisterCollection> _mixin;

		public InputRegisterCollection(params ushort[] registers)
			: this((IList<ushort>) registers)
		{
		}

		public InputRegisterCollection(IList<ushort> registers)
			: base(registers)
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
