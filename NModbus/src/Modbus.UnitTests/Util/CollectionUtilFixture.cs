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
		public void SliceMiddle()
		{
			byte[] test = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
			Assert.AreEqual(new byte[] { 3, 4, 5, 6, 7 }, CollectionUtil.Slice<byte>(test, 2, 5));	
		}

		[Test]
		public void SliceBeginning()
		{
			byte[] test = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
			Assert.AreEqual(new byte[] { 1, 2 }, CollectionUtil.Slice<byte>(test, 0, 2));						
		}

		[Test]
		public void SliceEnd()
		{
			byte[] test = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
			Assert.AreEqual(new byte[] { 9, 10 }, CollectionUtil.Slice<byte>(test, 8, 2));
		}

		[Test]
		public void SliceCollection()
		{
			Collection<bool> col = new Collection<bool>(new bool[] { true, false, false, false, true, true });
			Assert.AreEqual(new bool[] { false, false, true }, CollectionUtil.Slice<bool>(col, 2, 3));			
		}

		[Test]
		public void SliceReadOnlyCollection()
		{
			ReadOnlyCollection<bool> col = new ReadOnlyCollection<bool>(new bool[] { true, false, false, false, true, true });
			Assert.AreEqual(new bool[] { false, false, true }, CollectionUtil.Slice<bool>(col, 2, 3));
		}

		[Test]
		public void ToArray()
		{
			Collection<bool> col = new Collection<bool>(new bool[] { true, false, false, false, true, true });
			Assert.AreEqual(new bool[] { true, false, false, false, true, true }, CollectionUtil.ToArray<bool>(col));
		}

		[Test]
		public void ToArrayEmpty()
		{
			Assert.AreEqual(new bool[] { }, CollectionUtil.ToArray(new List<bool>(new bool[] { })));
		}

		[Test]
		public void ToBoolArray()
		{
			Assert.AreEqual(new bool[] { true, false, true }, CollectionUtil.ToBoolArray(new BitArray(new bool[] { true, false, true })));
		}

		[Test]
		public void ToBoolArrayEmpty()
		{
			Assert.AreEqual(new bool[] { }, CollectionUtil.ToBoolArray(new BitArray(0, false)));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ToBoolArrayNull()
		{
			CollectionUtil.ToBoolArray(null);
		}

		[Test]
		public void Combine()
		{
			Assert.AreEqual(new byte[] { 1, 2, 3, 4 }, CollectionUtil.Combine(new byte[] { 1, 2 }, new byte[] { 3, 4 }));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CombineNullArgument1()
		{
			CollectionUtil.Combine(null, new byte[] { 1, 2 });
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CombineNullArgument2()
		{
			CollectionUtil.Combine(new byte[] { 1, 2 }, null);
		}
	}
}
