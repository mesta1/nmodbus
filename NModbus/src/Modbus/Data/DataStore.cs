using System;
using System.Collections.Generic;
using System.Text;

namespace Modbus.Data
{
	public class DataStore
	{
		private DiscreteCollection _coilDiscretes = new DiscreteCollection();
		private DiscreteCollection _inputDiscetes = new DiscreteCollection();
		private RegisterCollection _holdingRegisters = new RegisterCollection();
		private RegisterCollection _inputRegisters = new RegisterCollection();

		public DataStore()
		{
		}

		public DiscreteCollection CoilDiscretes
		{
			get { return _coilDiscretes; }
			set { _coilDiscretes = value; }
		}

		public DiscreteCollection InputDiscretes
		{
			get { return _inputDiscetes; }
			set { _inputDiscetes = value; }
		}
	
	}
}
