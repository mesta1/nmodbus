using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Modbus.Utility;
using NUnit.Framework;

namespace Modbus.UnitTests.Utility
{
	[TestFixture]
	public class UtilityExtensionsFixture
	{		
		[Test]
		public void IsNullOrEmpty_EmptyString()
		{
			Assert.IsTrue("".IsNullOrEmpty());
		}

		[Test]
		public void IsNullOrEmpty_NonEmptyString()
		{
			Assert.IsFalse("foo".IsNullOrEmpty());
		}

		[Test]
		public void IsNullOrEmpty_Null()
		{
			string foo = null;
			Assert.IsTrue(foo.IsNullOrEmpty());
		}

		[Test]
		public void Concat()
		{
			Assert.AreEqual(new byte[] { 1, 2, 3, 4 }, new byte[] { 1, 2 }.Concat(new byte[] { 3, 4 }).ToArray());
		}

		[Test]
		public void Concat_ThreeArrays()
		{
			Assert.AreEqual(new byte[] { 1, 2, 3, 4, 5, 6 }, new byte[] { 1, 2 }.Concat(new byte[] { 3, 4 }, new byte[] { 5, 6 }).ToArray());
		}

		[Test]
		public void Concat_FourArrays()
		{
			Assert.AreEqual(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }, new byte[] { 1, 2 }.Concat(new byte[] { 3, 4 }, new byte[] { 5, 6 }, new byte[] { 7, 8 }).ToArray());
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Concat_NullParams()
		{
			new byte[] { 1, 2 }.Concat(new byte[] { 3, 4 }, null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Concat_NullArgument1()
		{
			new byte[] { }.Concat(null, new byte[] { 1, 2 });
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Concat_NullArgument2()
		{
			new byte[] { }.Concat(new byte[] { 1, 2 }, null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Concat_NullArgument3()
		{
			new byte[] { }.Concat(new byte[] { 1, 2 }, new byte[] { 3, 4 }, null);
		}

		[Test]
		public void ToSequence()
		{
			Assert.AreEqual(new byte[] { 1, 2, 3 }, 1.ToSequence(2, 3).ToArray());
		}

		[Test]
		public void ToSequence_SingleItem()
		{
			Assert.AreEqual(new byte[] { 1 }, 1.ToSequence().ToArray());
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void ToSequence_Null()
		{
			new object().ToSequence(null).ToArray();
		}

		[Test]
		public void Slice()
		{
			Assert.AreEqual(new int[] { 3, 4, 5 }, new int[] { 1, 2, 3, 4, 5 }.Slice(2, 3).ToArray());
		}

		[Test]
		public void Slice_StartIndexMax()
		{
			Assert.AreEqual(new int[] { 5 }, new int[] { 1, 2, 3, 4, 5 }.Slice(4, 1).ToArray());
		}

		[Test]
		public void Slice_EntireSource()
		{
			Assert.AreEqual(new int[] { 1, 2, 3, 4, 5 }, new int[] { 1, 2, 3, 4, 5 }.Slice(0, 5).ToArray());
		}

		[Test]
		public void Slice_Empty()
		{
			Assert.AreEqual(new int[] { }, new int[] { }.Slice(0, 0).ToArray());
		}

		[Test]
		public void Slice_One()
		{
			Assert.AreEqual(new int[] { 3 }, new int[] { 3 }.Slice(0, 1).ToArray());
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void Slice_SizeTooLarge()
		{
			int[] result = new int[] { 1, 2, 3, 4, 5 }.Slice(2, 4).ToArray();
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void Slice_SizeNegative()
		{
			new int[] { 1, 2, 3, 4, 5 }.Slice(2, -1);
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void Slice_StartIndexNegative()
		{
			new int[] { 1, 2, 3, 4, 5 }.Slice(-1, 1);
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void Slice_StartIndexTooLarge()
		{
			new int[] { 1, 2, 3, 4, 5 }.Slice(5, 1);
		}

		[Test]
		public void JoinArray()
		{
			ushort[] registers = new ushort[] { 1, 2, 3 };
			Assert.AreEqual("1, 2, 3", registers.Join(", "));
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void JoinArrayNull()
		{
			bool[] array = null;
			array.Join(", ");
			Assert.Fail();
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void JoinArrayConverterNull()
		{
			new bool[] { true, false }.Join(", ", null);
			Assert.Fail();
		}

		[Test]
		public void JoinCollection()
		{
			Collection<ushort> registers = new Collection<ushort>(new ushort[] { 1, 2, 3 });
			Assert.AreEqual("1, 2, 3", registers.Join(", "));
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void Join_CollectionNull()
		{
			ICollection<bool> col = null;
			col.Join(", ");
			Assert.Fail();
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void Join_CollectionConverterNull()
		{
			new Collection<ushort>(new ushort[] { 1 }).Join(", ", null);
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void Join_SeparatorNull()
		{
			new int[] { }.Join(null);
		}

		[Test]
		public void Join_SeparatorEmpty()
		{
			Assert.AreEqual("12", new int[] { 1, 2 }.Join(""));
		}

		[Test]
		public void JoinArrayCustomConversion()
		{
			ushort[] registers = new ushort[] { 1, 2, 3 };
			Assert.AreEqual("number: 1, number: 2, number: 3", registers.Join(", ", delegate(ushort number) { return String.Format("number: {0}", number); }));
		}

		[Test]
		public void JoinCollectionCustomConversion()
		{
			Collection<ushort> registers = new Collection<ushort>(new ushort[] { 1, 2, 3 });
			Assert.AreEqual("number: 1, number: 2, number: 3", registers.Join(", ", delegate(ushort number) { return String.Format("number: {0}", number); }));
		}

		[Test]
		public void ForEach()
		{
			var array = new int[] { 1, 2, 3 };
			int sum = 0;
			array.ForEach((n) => sum += n);

			Assert.AreEqual(6, sum);
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void ForEach_ActionNull()
		{
			new int[] { 1, 2, 3 }.ForEach(null);
		}

		[Test]
		public void ForEachWithIndex()
		{
			var array = new string[] { "one", "two", "three" };
			int expectedIndex = 0;
			array.ForEachWithIndex((value, index) => Assert.AreEqual(expectedIndex++, index));
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void ForEachWithIndex_SourceNull()
		{
			string[] array = null;
			array.ForEachWithIndex((value, index) => Assert.Fail());
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void ForEachWithIndex_ActionNull()
		{
			new string[] { }.ForEachWithIndex(null);
		}

		[Test]
		public void WithIndex()
		{
			var array = new string[] { "one", "two", "three" };
			int expectedIndex = 0;

			foreach (var pair in array.WithIndex())
			{
				Assert.AreEqual(pair.Value, array[expectedIndex]);
				Assert.AreEqual(pair.Index, expectedIndex++);
			}
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void WithIndex_SourceNull()
		{
			string[] array = null;
			array.WithIndex().ToArray();
		}

		[Test]
		public void IfNotNullAction_Null()
		{
			object test = null;
			test.IfNotNull(t => Assert.Fail());
		}

		[Test]
		public void IfNotNullAction_Success()
		{
			int test = 5;
			int foo = 0;
			test.IfNotNull(t => foo = t);
			Assert.AreEqual(test, foo);
		}

		[Test]
		public void IfNotNullFunc_Null()
		{
			string test = null;
			Assert.AreEqual(null, test.IfNotNull(t => "junk"));
		}

		[Test]
		public void IfNotNullFunc_Success()
		{
			int test = 5;
			Assert.AreEqual(5, test.IfNotNull(t => t));
		}
	}
}
