using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Data;
using System.Collections; 

namespace Modbus.UnitTests.Data
{
	[TestFixture]
	public class DiscreteMixinFixture
	{
		[Test]
		public void CheckGetBytes()
		{
			DiscreteMixin<BitArray> col = new DiscreteMixin<BitArray>();
			BitArray bitArray = new BitArray(new byte[] { 1, 1 });
			Assert.AreEqual(new byte[] { 1, 1 }, col.GetBytes(bitArray));
		}		
	}
}
