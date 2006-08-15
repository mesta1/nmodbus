using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Data;

namespace Modbus.UnitTests.Data
{
	[TestFixture]
	public class InputDiscreteCollectionFixture
	{
		[Test]
		public void CreateNewCoilDiscreteCollectionFromBoolParams()
		{
			InputDiscreteCollection col = new InputDiscreteCollection(true, false, true);
			Assert.AreEqual(3, col.Count);
		}

		[Test]
		public void CreateNewCoilDiscreteCollectionFromBytesParams()
		{
			InputDiscreteCollection col = new InputDiscreteCollection(1, 2, 3);
			Assert.AreEqual(24, col.Count);
		}
	}
}
