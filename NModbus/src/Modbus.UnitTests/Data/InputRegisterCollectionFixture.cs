using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Data;

namespace Modbus.UnitTests.Data
{
	[TestFixture]
	public class InputRegisterCollectionFixture
	{
		[Test]
		public void CheckInputRegisterFromByteParams()
		{
			InputRegisterCollection col = new InputRegisterCollection(5, 3, 4, 6);
			byte[] bytes = col.Bytes;
			Assert.IsNotNull(bytes);
			Assert.AreEqual(8, bytes.Length);
			Assert.AreEqual(new byte[] { 5, 0, 3, 0, 4, 0, 6, 0 }, bytes);
		}

		[Test]
		public void CheckInputRegisterEmpty()
		{
			InputRegisterCollection col = new InputRegisterCollection();
			Assert.IsNotNull(col);
			Assert.AreEqual(0, col.Bytes.Length);
		}
	}
}
