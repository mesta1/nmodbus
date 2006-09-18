using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Util;
using NUnit.Framework;
using Modbus.Message;

namespace Modbus.UnitTests.Util
{
	[TestFixture]
	public class ModbusUtilFixture
	{
		[Test]
		public void GetASCIIBytesEmpty()
		{
			Assert.AreEqual(new byte[] { }, ModbusUtil.GetAsciiBytes(new byte[] { }));
			Assert.AreEqual(new byte[] { }, ModbusUtil.GetAsciiBytes(new int[] { }));
		}

		[Test]
		public void GetASCIIBytesNormal()
		{
			byte[] buf = new byte[] { 2, 5 };
			byte[] expectedResult = new byte[] { 48, 50, 48, 53 };
			byte[] result = ModbusUtil.GetAsciiBytes(buf);
			Assert.AreEqual(expectedResult, result);
		}

		[Test]
		public void GetASCIIBytesFromIntNormal()
		{
			int[] buf = new int[] { 300, 400 };
			byte[] expectedResult = new byte[] { 48, 49, 50, 67, 48, 49, 57, 48 };
			byte[] result = ModbusUtil.GetAsciiBytes(buf);
			Assert.AreEqual(expectedResult, result);			
		}

		[Test]
		public void GetASCIIBytesFromCharNormal()
		{
			char[] buf = new char[] { ':' };
			byte[] expectedResult = new byte[] { 58 };
			byte[] result = ModbusUtil.GetAsciiBytes(buf);
			Assert.AreEqual(expectedResult, result);
		}

		[Test]
		public void GetASCIIBytesFromCharNormal2()
		{
			char[] buf = new char[] { '\r', '\n' };
			byte[] expectedResult = new byte[] { 13, 10 };
			byte[] result = ModbusUtil.GetAsciiBytes(buf);
			Assert.AreEqual(expectedResult, result);
		}

		[Test]
		public void HexToBytes()
		{
			Assert.AreEqual(new byte[] { 255 }, ModbusUtil.HexToBytes("FF"));	
		}

		[Test]
		public void HexToBytes2()
		{
			Assert.AreEqual(new byte[] { 204, 255}, ModbusUtil.HexToBytes("CCFF"));
		}

		[Test]
		public void HexToBytesEmpty()
		{
			Assert.AreEqual(new byte[] { }, ModbusUtil.HexToBytes(""));			
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void HexToBytesNull()
		{
			ModbusUtil.HexToBytes(null);
			Assert.Fail();
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void HexToBytesOdd()
		{
			ModbusUtil.HexToBytes("CCF");
			Assert.Fail();
		}

		[Test]
		public void CalculateCrc()
		{
			byte[] result = ModbusUtil.CalculateCrc(new byte[] { 1, 1 });
			Assert.AreEqual(new byte[] { 193, 224 }, result);			
		}

		[Test]
		public void CalculateCrc2()
		{
			byte[] result = ModbusUtil.CalculateCrc(new byte[] { 2, 1, 5, 0 });
			Assert.AreEqual(new byte[] { 83, 12 }, result);
		}

		[Test]
		public void CalculateCrcEmpty()
		{
			Assert.AreEqual(new byte[] { 255, 255 }, ModbusUtil.CalculateCrc(new byte[] { }));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CalculateCrcNull()
		{
			ModbusUtil.CalculateCrc(null);
			Assert.Fail();
		}

		[Test]
		public void CalculateLrc()
		{
			ReadCoilsInputsRequest request = new ReadCoilsInputsRequest(Modbus.ReadCoils, 1, 1, 10);
			Assert.AreEqual(243, ModbusUtil.CalculateLrc(new byte[] { 1, 1, 0, 1, 0, 10 }));
		}

		[Test]
		public void CalculateLrc2()
		{
			//: 02 01 0000 0001 FC
			ReadCoilsInputsRequest request = new ReadCoilsInputsRequest(Modbus.ReadCoils, 2, 0, 1);			
			Assert.AreEqual(252, ModbusUtil.CalculateLrc(new byte[] { 2, 1, 0, 0, 0, 1}));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CalculateLrcNull()
		{
			ModbusUtil.CalculateLrc(null);
			Assert.Fail();
		}

		[Test]
		public void CalculateLrcEmpty()
		{
			Assert.AreEqual(0, ModbusUtil.CalculateLrc(new byte[] {}));
		}

		[Test]
		public void NetworkBytesToHostUInt16()
		{
			Assert.AreEqual(new ushort[] { 1, 2 }, ModbusUtil.NetworkBytesToHostUInt16(new byte[] { 0, 1, 0, 2 }));
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void ByteArrayToUInt16ArrayOddNumberOfBytes()
		{
			ModbusUtil.NetworkBytesToHostUInt16(new byte[] { 1 });
			Assert.Fail();
		}

		[Test]
		public void ByteArrayToUInt16ArrayEmptyBytes()
		{
			Assert.AreEqual(new ushort[] { }, ModbusUtil.NetworkBytesToHostUInt16(new byte[] { }));
		}
	}
}
