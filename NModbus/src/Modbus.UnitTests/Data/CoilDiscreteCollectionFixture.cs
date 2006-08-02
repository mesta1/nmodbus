using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Data;
using System.Collections;
using Modbus.Util;

namespace Modbus.UnitTests.Data
{
	[TestFixture]
	public class CoilDiscreteCollectionFixture
	{
		[Test]
		public void CheckCreateNewCoilDiscreteCollectionFromBoolParams()
		{
			CoilDiscreteCollection col = new CoilDiscreteCollection(true, false, true);
			Assert.AreEqual(3, col.Count);
		}

		[Test]
		public void CheckCreateNewCoilDiscreteCollectionFromBytesParams()
		{
			CoilDiscreteCollection col = new CoilDiscreteCollection(1, 2, 3);
			Assert.AreEqual(24, col.Count);
		}

		[Test]
		public void CheckCreateNewCoilDiscreteCollectionFromBytesParamsOrder()
		{
			CoilDiscreteCollection col = new CoilDiscreteCollection(194);
			Assert.AreEqual(new bool[] { false, true, false, false, false, false, true, true }, CollectionUtil.ToArray(col));
		}

		[Test]
		public void CheckCreateNewCoilDiscreteCollectionFromBytesParamsOrder2()
		{
			CoilDiscreteCollection col = new CoilDiscreteCollection(157, 7);
			Assert.AreEqual(new bool[] { true, false, true, true, true, false, false, true, true, true, true, false, false, false, false, false }, CollectionUtil.ToArray(col));
		}

		[Test]
		public void CheckResize()
		{
			CoilDiscreteCollection col = new CoilDiscreteCollection(byte.MaxValue, byte.MaxValue);
			Assert.AreEqual(16, col.Count);
			col.RemoveAt(3);
			Assert.AreEqual(15, col.Count);
		}

		[Test]
		public void CheckBytesPersistence()
		{
			CoilDiscreteCollection col = new CoilDiscreteCollection(byte.MaxValue, byte.MaxValue);
			Assert.AreEqual(16, col.Count);
			byte[] originalBytes = col.NetworkBytes;
			col.RemoveAt(3);
			Assert.AreEqual(15, col.Count);
			Assert.AreNotEqual(originalBytes, col.NetworkBytes);
		}

		[Test]
		public void CheckAddCoil()
		{
			CoilDiscreteCollection col = new CoilDiscreteCollection();
			Assert.AreEqual(0, col.Count);
			col.Add(true);
			Assert.AreEqual(1, col.Count);
		}
	}
}
