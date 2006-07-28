using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Util;
using NUnit.Framework;

namespace Modbus.UnitTests.Util
{
	[TestFixture]
	public class ModbusUtilFixture
	{
		[Test]
		public void CheckGetASCIIBytesEmpty()
		{
			Assert.AreEqual(new byte[] { }, ModbusUtil.GetASCIIBytes(new byte[] { }));
			Assert.AreEqual(new byte[] { }, ModbusUtil.GetASCIIBytes(new int[] { }));
		}

		[Test]
		public void CheckGetASCIIBytesNormal()
		{
			byte[] buf = new byte[] { 2, 5 };
			byte[] expectedResult = new byte[] { 48, 50, 48, 53 };
			byte[] result = ModbusUtil.GetASCIIBytes(buf);
			Assert.AreEqual(expectedResult, result);
		}

		[Test]
		public void CheckGetASCIIBytesFromIntNormal()
		{
			int[] buf = new int[] { 300, 400 };
			byte[] expectedResult = new byte[] { 48, 49, 50, 67, 48, 49, 57, 48 };
			byte[] result = ModbusUtil.GetASCIIBytes(buf);
			Assert.AreEqual(expectedResult, result);			
		}

		[Test]
		public void CheckGetASCIIBytesFromCharNormal()
		{
			char[] buf = new char[] { ':' };
			byte[] expectedResult = new byte[] { 58 };
			byte[] result = ModbusUtil.GetASCIIBytes(buf);
			Assert.AreEqual(expectedResult, result);
		}

		[Test]
		public void CheckGetASCIIBytesFromCharNormal2()
		{
			char[] buf = new char[] { '\r', '\n' };
			byte[] expectedResult = new byte[] { 13, 10 };
			byte[] result = ModbusUtil.GetASCIIBytes(buf);
			Assert.AreEqual(expectedResult, result);
		}

		[Test]
		public void CheckHexToBytes()
		{
			Assert.AreEqual(new byte[] { 255 }, ModbusUtil.HexToBytes("FF"));	
		}

		[Test]
		public void CheckHexToBytes2()
		{
			Assert.AreEqual(new byte[] { 204, 255}, ModbusUtil.HexToBytes("CCFF"));
		}

		[Test]
		public void CheckHexToBytesEmpty()
		{
			Assert.AreEqual(new byte[] { }, ModbusUtil.HexToBytes(""));			
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CheckHexToBytesNull()
		{
			ModbusUtil.HexToBytes(null);
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void CheckHexToBytesOdd()
		{
			ModbusUtil.HexToBytes("CCF");
		}

		[Test]
		public void CheckCalculateCRC()
		{
			byte[] result = ModbusUtil.CalculateCRC(new byte[] { 1, 1 });
			Assert.AreEqual(new byte[] { 193, 224 }, result);			
		}

		[Test]
		public void CheckCalculateCRC2()
		{
			byte[] result = ModbusUtil.CalculateCRC(new byte[] { 2, 1, 5, 0 });
			Assert.AreEqual(new byte[] { 83, 12 }, result);
		}

		[Test]
		public void CheckCalculateCRCEmpty()
		{
			byte[] result = ModbusUtil.CalculateCRC(new byte[] { });
			Assert.AreEqual(new byte[] { 255, 255 }, result);			
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CheckCalculateCRCNull()
		{
			ModbusUtil.CalculateCRC(null);
		}

		[Test]
		public void CheckNetworkBytesToHostUInt16()
		{
			Assert.AreEqual(new ushort[] { 1, 2 }, ModbusUtil.NetworkBytesToHostUInt16(new byte[] { 0, 1, 0, 2 }));
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void CheckByteArrayToUInt16ArrayOddNumberOfBytes()
		{
			ModbusUtil.NetworkBytesToHostUInt16(new byte[] { 1 });
		}

		[Test]
		public void CheckByteArrayToUInt16ArrayEmptyBytes()
		{
			Assert.AreEqual(new ushort[] { }, ModbusUtil.NetworkBytesToHostUInt16(new byte[] { }));
		}
	}
}
