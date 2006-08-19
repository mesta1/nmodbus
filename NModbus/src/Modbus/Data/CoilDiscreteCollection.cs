using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Modbus.Util;

namespace Modbus.Data
{
	class CoilDiscreteCollection : Collection<bool>, DiscreteMixin<CoilDiscreteCollection>.IMixin
	{
		DiscreteMixin<CoilDiscreteCollection> _mixin;
		
		public CoilDiscreteCollection ()
		{
		}

		public CoilDiscreteCollection(params bool[] bits)
			: this((IList<bool>)bits)
		{
		}

		public CoilDiscreteCollection(params byte[] bytes)
			: this((IList<bool>)CollectionUtil.ToBoolArray(new BitArray(bytes)))
		{
		}

		public CoilDiscreteCollection(IList<bool> bits)
		    : base(bits.IsReadOnly ? new List<bool>(bits) : bits)
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
