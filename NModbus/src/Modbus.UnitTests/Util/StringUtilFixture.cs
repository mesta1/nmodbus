using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Modbus.Util;
using NUnit.Framework;

namespace Modbus.UnitTests.Util
{
	[TestFixture]
	public class StringUtilFixture
	{
		[Test]
		public void JoinArray()
		{
			ushort[] registers = new ushort[] { 1, 2, 3 };
			Assert.AreEqual("1, 2, 3", StringUtility.Join(", ", registers));
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void JoinArrayNull()
		{
			bool[] array = null;
			StringUtility.Join(", ", array);
			Assert.Fail();
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void JoinArrayConverterNull()
		{
			StringUtility.Join(", ", new bool[] { true, false }, null);
			Assert.Fail();
		}

		[Test]
		public void JoinCollection()
		{
			Collection<ushort> registers = new Collection<ushort>(new ushort[] { 1, 2, 3 });
			Assert.AreEqual("1, 2, 3", StringUtility.Join(", ", registers));
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void JoinCollectionNull()
		{
			ICollection<bool> col = null;
			StringUtility.Join(", ", col);
			Assert.Fail();
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void JoinCollectionConverterNull()
		{
			StringUtility.Join(", ", new Collection<ushort>(new ushort[] { 1 }), null);
			Assert.Fail();
		}

		[Test]
		public void JoinArrayCustomConversion()
		{
			ushort[] registers = new ushort[] { 1, 2, 3 };
			Assert.AreEqual("number: 1, number: 2, number: 3", StringUtility.Join(", ", registers, delegate(ushort number) { return String.Format("number: {0}", number); }));
		}


		[Test]
		public void JoinCollectionCustomConversion()
		{
			Collection<ushort> registers = new Collection<ushort>(new ushort[] { 1, 2, 3 });
			Assert.AreEqual("number: 1, number: 2, number: 3", StringUtility.Join(", ", registers, delegate(ushort number) { return String.Format("number: {0}", number); }));
		}
	}
}
