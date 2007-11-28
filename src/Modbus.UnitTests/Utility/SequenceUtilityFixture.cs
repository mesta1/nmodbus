using System;
using NUnit.Framework;
using Modbus.Utility;
using System.Collections;
using System.Collections.Generic;

namespace Modbus.UnitTests.Utility
{
	[TestFixture]
	public class SequenceUtilityFixture
	{
		[Test]
		public void ToList()
		{
			IList<int> list = SequenceUtility.ToList(TestIterator);
			Assert.AreEqual(5, list.Count);
			Assert.AreEqual(4, list[4]);
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void ToList_NullSequence()
		{
			SequenceUtility.ToList<int>(null);
		}

		private IEnumerable<int> TestIterator
		{
			get
			{
				for (int i = 0; i < 5; i++)
					yield return i;
			}
		}
	}
}
