using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Modbus.Utility;

namespace Modbus.UnitTests.Utility
{
	[TestFixture]
	public class UtilityExtensionsFixture
	{
		[Test]
		public void ForEach()
		{
			var array = new int[] { 1, 2, 3 };
			int sum = 0;
			array.ForEach((n) => sum += n);

			Assert.AreEqual(6, sum);
		}

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
	}
}
