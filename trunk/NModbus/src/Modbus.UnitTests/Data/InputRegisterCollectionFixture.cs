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
		public void InputRegisterCollectionNetworkBytes()
		{
			InputRegisterCollection col = new InputRegisterCollection(5, 3, 4, 6);
			byte[] bytes = col.NetworkBytes;
			Assert.IsNotNull(bytes);
			Assert.AreEqual(8, bytes.Length);
			Assert.AreEqual(new byte[] { 0, 5, 0, 3, 0, 4, 0, 6 }, bytes);
		}

		[Test]
		public void NewInputRegisterCollectionFromByteArray()
		{
			InputRegisterCollection col = new InputRegisterCollection(new byte[] { 2, 43, 0, 0, 0, 100 });
			Assert.AreEqual(3, col.Count);
			Assert.AreEqual(555, col[0]);
			Assert.AreEqual(0, col[1]);
			Assert.AreEqual(100, col[2]);
		}

		[Test]
		public void InputRegisterCollectionEmpty()
		{
			InputRegisterCollection col = new InputRegisterCollection();
			Assert.IsNotNull(col);
			Assert.AreEqual(0, col.NetworkBytes.Length);
		}
	}
}
