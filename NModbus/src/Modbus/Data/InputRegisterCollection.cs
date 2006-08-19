using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Modbus.Util;

namespace Modbus.Data
{
	class InputRegisterCollection : ReadOnlyCollection<ushort>, IModbusMessageDataCollection, RegisterMixin<InputRegisterCollection>.IMixin
	{
		RegisterMixin<InputRegisterCollection> _mixin;

		public InputRegisterCollection(byte[] bytes)
			: this((IList<ushort>) ModbusUtil.NetworkBytesToHostUInt16(bytes))
		{
		}

		public InputRegisterCollection(params ushort[] registers)
			: this((IList<ushort>) registers)
		{
		}

		public InputRegisterCollection(IList<ushort> registers)
			: base(registers)
		{
		}

		public byte[] NetworkBytes
		{
			get
			{
				return _mixin.GetNetworkBytes(this);
			}
		}
	}
}
