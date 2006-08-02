using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Data;
using System.Net;

namespace Modbus.UnitTests.Data
{
	[TestFixture]
	public class HoldingRegisterCollectionFixture
	{
		[Test]
		public void NewHoldingRegisterCollection()
		{
			HoldingRegisterCollection col = new HoldingRegisterCollection(5, 3, 4, 6);
			Assert.IsNotNull(col);
			Assert.AreEqual(4, col.Count);
			Assert.AreEqual(5, col[0]);
		}

		[Test]
		public void NewHoldingRegisterCollectionFromBytes()
		{
			HoldingRegisterCollection col = new HoldingRegisterCollection(new byte[] { 0, 1, 0, 2, 0, 3 });
			Assert.IsNotNull(col);
			Assert.AreEqual(3, col.Count);
			Assert.AreEqual(1, col[0]);
			Assert.AreEqual(2, col[1]);
			Assert.AreEqual(3, col[2]);
		}

		[Test]
		public void HoldingRegisterCollectionNetworkBytes()
		{
			HoldingRegisterCollection col = new HoldingRegisterCollection(5, 3, 4, 6);
			byte[] bytes = col.NetworkBytes;
			Assert.IsNotNull(bytes);
			Assert.AreEqual(8, bytes.Length);
			Assert.AreEqual(new byte[] { 0, 5, 0, 3, 0, 4, 0, 6 }, bytes);
		}

		[Test]
		public void HoldingRegisterCollectionEmpty()
		{
			HoldingRegisterCollection col = new HoldingRegisterCollection();
			Assert.IsNotNull(col);
			Assert.AreEqual(0, col.NetworkBytes.Length);			
		}

		[Test]
		public void ModifyRegister()
		{
			HoldingRegisterCollection col = new HoldingRegisterCollection(1, 2, 3, 4);
			col[0] = 5;
		}

		[Test]
		public void AddRegister()
		{
			HoldingRegisterCollection col = new HoldingRegisterCollection();
			Assert.AreEqual(0, col.Count);
			col.Add(45);
			Assert.AreEqual(1, col.Count);
		}

		[Test]
		public void RemoveRegister()
		{
			HoldingRegisterCollection col = new HoldingRegisterCollection(3, 4, 5);
			Assert.AreEqual(3, col.Count);
			col.RemoveAt(2);
			Assert.AreEqual(2, col.Count);
		}
	}
}
