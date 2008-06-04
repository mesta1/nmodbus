using System;
using Modbus.Data;
using Modbus.Message;
using Modbus.Utility;
using MbUnit.Framework;

namespace Modbus.UnitTests.Message
{
	[TestFixture]
	public class WriteMultipleRegistersRequestFixture
	{
		[Test]
		public void CreateWriteMultipleRegistersRequestFixture()
		{
			RegisterCollection col = new RegisterCollection(10, 20, 30, 40, 50);
			WriteMultipleRegistersRequest request = new WriteMultipleRegistersRequest(11, 34, col);
			Assert.AreEqual(Modbus.WriteMultipleRegisters, request.FunctionCode);
			Assert.AreEqual(11, request.SlaveAddress);
			Assert.AreEqual(34, request.StartAddress);
			Assert.AreEqual(10, request.ByteCount);
			Assert.AreEqual(col.NetworkBytes, request.Data.NetworkBytes);
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void CreateWriteMultipleRegistersRequestTooMuchData()
		{
			new WriteMultipleRegistersRequest(1, 2, CollectionUtility.CreateDefaultCollection<RegisterCollection, ushort>(3, Modbus.MaximumRegisterRequestResponseSize + 1));
		}

		[Test]
		public void CreateWriteMultipleRegistersRequestMaxSize()
		{
			WriteMultipleRegistersRequest request = new WriteMultipleRegistersRequest(1, 2, CollectionUtility.CreateDefaultCollection<RegisterCollection, ushort>(3, Modbus.MaximumRegisterRequestResponseSize));
			Assert.AreEqual(Modbus.MaximumRegisterRequestResponseSize, request.NumberOfPoints);
		}
	}
}
