using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Modbus.Util;

namespace Modbus.Data
{
	public class InputDiscreteCollection : ReadOnlyCollection<bool>, IModbusMessageDataCollection, DiscreteMixin<InputDiscreteCollection>.IMixin
	{
		DiscreteMixin<InputDiscreteCollection> _mixin;

		public InputDiscreteCollection(params byte[] bytes)
			: this((IList<bool>) CollectionUtil.ToBoolArray(new BitArray(bytes)))
		{
		}

		public InputDiscreteCollection(params bool[] bits)
			: this((IList<bool>) bits)
		{
		}

		public InputDiscreteCollection(IList<bool> bits)
			: base(bits)
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
