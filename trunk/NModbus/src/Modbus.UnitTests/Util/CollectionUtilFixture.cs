using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Modbus.Util;
using System.Collections.ObjectModel;
using System.Collections;

namespace Modbus.UnitTests.Util
{
	[TestFixture]
	public class CollectionUtilFixture
	{
		[Test]
		public void CheckSliceMiddle()
		{
			byte[] test = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
			Assert.AreEqual(new byte[] { 3, 4, 5, 6, 7 }, CollectionUtil.Slice<byte>(test, 2, 5));	
		}

		[Test]
		public void CheckSliceBeginning()
		{
			byte[] test = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
			Assert.AreEqual(new byte[] { 1, 2 }, CollectionUtil.Slice<byte>(test, 0, 2));						
		}

		[Test]
		public void CheckSliceEnd()
		{
			byte[] test = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
			Assert.AreEqual(new byte[] { 9, 10 }, CollectionUtil.Slice<byte>(test, 8, 2));
		}

		[Test]
		public void CheckSliceCollection()
		{
			Collection<bool> col = new Collection<bool>(new bool[] { true, false, false, false, true, true });
			Assert.AreEqual(new bool[] { false, false, true }, CollectionUtil.Slice<bool>(col, 2, 3));			
		}

		[Test]
		public void CheckSliceReadOnlyCollection()
		{
			ReadOnlyCollection<bool> col = new ReadOnlyCollection<bool>(new bool[] { true, false, false, false, true, true });
			Assert.AreEqual(new bool[] { false, false, true }, CollectionUtil.Slice<bool>(col, 2, 3));
		}

		[Test]
		public void CheckToArray()
		{
			Collection<bool> col = new Collection<bool>(new bool[] { true, false, false, false, true, true });
			Assert.AreEqual(new bool[] { true, false, false, false, true, true }, CollectionUtil.ToArray<bool>(col));
		}

		[Test]
		public void CheckToArrayEmpty()
		{
			Assert.AreEqual(new bool[] { }, CollectionUtil.ToArray(new List<bool>(new bool[] { })));
		}

		[Test]
		public void CheckToBoolArray()
		{
			Assert.AreEqual(new bool[] { true, false, true }, CollectionUtil.ToBoolArray(new BitArray(new bool[] { true, false, true })));
		}

		[Test]
		public void CheckToBoolArrayEmpty()
		{
			Assert.AreEqual(new bool[] { }, CollectionUtil.ToBoolArray(new BitArray(0, false)));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CheckToBoolArrayNull()
		{
			CollectionUtil.ToBoolArray(null);
		}
	}
}
