using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Modbus.Data;
using Modbus.Utility;
using NUnit.Framework;

namespace Modbus.UnitTests.Utility
{
	[TestFixture]
	public class CollectionUtilityFixture
	{
		[Test]
		public void SliceMiddle()
		{
			byte[] test = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
			Assert.AreEqual(new byte[] { 3, 4, 5, 6, 7 }, CollectionUtility.Slice<byte>(test, 2, 5));	
		}

		[Test]
		public void SliceBeginning()
		{
			byte[] test = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
			Assert.AreEqual(new byte[] { 1, 2 }, CollectionUtility.Slice<byte>(test, 0, 2));						
		}

		[Test]
		public void SliceEnd()
		{
			byte[] test = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
			Assert.AreEqual(new byte[] { 9, 10 }, CollectionUtility.Slice<byte>(test, 8, 2));
		}

		[Test]
		public void SliceCollection()
		{
			Collection<bool> col = new Collection<bool>(new bool[] { true, false, false, false, true, true });
			Assert.AreEqual(new bool[] { false, false, true }, CollectionUtility.Slice<bool>(col, 2, 3));			
		}

		[Test]
		public void SliceReadOnlyCollection()
		{
			ReadOnlyCollection<bool> col = new ReadOnlyCollection<bool>(new bool[] { true, false, false, false, true, true });
			Assert.AreEqual(new bool[] { false, false, true }, CollectionUtility.Slice<bool>(col, 2, 3));
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void SliceNullICollection()
		{
			ICollection<bool> col = null;
			CollectionUtility.Slice<bool>(col, 1, 1);
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void SliceNullArray()
		{
			bool[] array = null;
			CollectionUtility.Slice<bool>(array, 1, 1);
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void ToArrayNull()
		{
			CollectionUtility.ToArray<bool>(null);
		}

		[Test]
		public void ToArray()
		{
			Collection<bool> col = new Collection<bool>(new bool[] { true, false, false, false, true, true });
			Assert.AreEqual(new bool[] { true, false, false, false, true, true }, CollectionUtility.ToArray<bool>(col));
		}

		[Test]
		public void ToArrayEmpty()
		{
			Assert.AreEqual(new bool[] { }, CollectionUtility.ToArray(new List<bool>(new bool[] { })));
		}

		[Test]
		public void ToBoolArray()
		{
			Assert.AreEqual(new bool[] { true, false, true }, CollectionUtility.ToBoolArray(new BitArray(new bool[] { true, false, true })));
		}

		[Test]
		public void ToBoolArrayEmpty()
		{
			Assert.AreEqual(new bool[] { }, CollectionUtility.ToBoolArray(new BitArray(0, false)));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ToBoolArrayNull()
		{
			CollectionUtility.ToBoolArray(null);
		}

		[Test]
		public void Combine()
		{
			Assert.AreEqual(new byte[] { 1, 2, 3, 4 }, CollectionUtility.Concat(new byte[] { 1, 2 }, new byte[] { 3, 4 }));
		}

		[Test]
		public void Combine_ThreeArrays()
		{
			Assert.AreEqual(new byte[] { 1, 2, 3, 4, 5, 6 }, CollectionUtility.Concat(new byte[] { 1, 2 }, new byte[] { 3, 4 }, new byte[] { 5, 6 }));
		}

		[Test]
		public void Combine_FourArrays()
		{
			Assert.AreEqual(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }, CollectionUtility.Concat(new byte[] { 1, 2 }, new byte[] { 3, 4 }, new byte[] { 5, 6 }, new byte[] { 7, 8 }));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Combine_NullParams()
		{
			CollectionUtility.Concat(new byte[] { 1, 2 }, new byte[] { 3, 4 }, null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Combine_SomeParamsWithOneNull()
		{
			CollectionUtility.Concat(new byte[] { 1, 2 }, new byte[] { 3, 4 }, new byte[] { 5, 6 }, null, new byte[] { 7, 8 });
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Combine_NullArgument1()
		{
			CollectionUtility.Concat(null, new byte[] { 1, 2 });
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Combine_NullArgument2()
		{
			CollectionUtility.Concat(new byte[] { 1, 2 }, null);
		}

		[Test]
		public void Update()
		{
			List<int> newItems = new List<int>(new int[] { 4, 5, 6 });
			List<int> destination = new List<int>(new int[] { 1, 2, 3, 7, 8, 9 });
			CollectionUtility.Update<int>(newItems, destination, 3);
			Assert.AreEqual(new int[] { 1, 2, 3, 4, 5, 6 }, destination.ToArray());
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void UpdateItemsTooLarge()
		{
			List<int> newItems = new List<int>(new int[] { 1, 2, 3, 7, 8, 9 });
			List<int> destination = new List<int>(new int[] { 4, 5, 6 });			
			CollectionUtility.Update<int>(newItems, destination, 3);
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void UpdateNegativeIndex()
		{	
			List<int> newItems = new List<int>(new int[] { 1, 2, 3, 7, 8, 9 });
			List<int> destination = new List<int>(new int[] { 4, 5, 6 });
			CollectionUtility.Update<int>(newItems, destination, -1);
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void CreateDefaultCollectionNegativeSize()
		{
			CollectionUtility.CreateDefaultCollection<RegisterCollection, ushort>(0, -1);
		}

		[Test]
		public void CreateDefaultCollection()
		{
			RegisterCollection col = CollectionUtility.CreateDefaultCollection<RegisterCollection, ushort>(3, 5);
			Assert.AreEqual(5, col.Count);
			Assert.AreEqual(new ushort[] { 3, 3, 3, 3, 3 }, CollectionUtility.ToArray(col));
		}
	}
}