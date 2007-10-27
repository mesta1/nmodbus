using System;
using System.Collections.Generic;
using System.Reflection;
using Modbus.Message;
using NUnit.Framework;

namespace Modbus.UnitTests.Message
{
	[TestFixture]
	public class ModbusMessageFixture
	{
		[Test]
		public void ProtocolDataUnitReadCoilsRequest()
		{
			ModbusMessage message = new ReadCoilsInputsRequest(Modbus.ReadCoils, 1, 100, 9);
			byte[] expectedResult = { Modbus.ReadCoils, 0, 100, 0, 9 };
			Assert.AreEqual(expectedResult, message.ProtocolDataUnit);
		}

		[Test]
		public void MessageFrameReadCoilsRequest()
		{
			ModbusMessage message = new ReadCoilsInputsRequest(Modbus.ReadCoils, 1, 2, 3);
			byte[] expectedMessageFrame = { 1, Modbus.ReadCoils, 0, 2, 0, 3 };
			Assert.AreEqual(expectedMessageFrame, message.MessageFrame);
		}

		[Test, Ignore("TODO: implement ToString for all messages.")]
		public void ModbusMessageToStringOverriden()
		{
			foreach (Type messageType in GetConcreteSubClasses("Modbus.dll", typeof(ModbusMessage)))
				Assert.IsNotNull(messageType.GetMethod("ToString", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly), String.Concat("No ToString override in message ", messageType.FullName));
		}

		internal static void AssertModbusMessagePropertiesAreEqual(IModbusMessage obj1, IModbusMessage obj2)
		{
			Assert.AreEqual(obj1.FunctionCode, obj2.FunctionCode);
			Assert.AreEqual(obj1.SlaveAddress, obj2.SlaveAddress);
			Assert.AreEqual(obj1.MessageFrame, obj2.MessageFrame);
			Assert.AreEqual(obj1.ProtocolDataUnit, obj2.ProtocolDataUnit);
		}

		private static Type[] GetConcreteSubClasses(string assemblyPath, Type baseClassType)
		{
			Assembly assembly = Assembly.LoadFrom(assemblyPath);
			List<Type> subClasses = new List<Type>();

			foreach (Type type in assembly.GetTypes())
			{
				if (type.IsSubclassOf(baseClassType) && !type.IsAbstract)
				{
					subClasses.Add(type);
				}
			}

			return (subClasses.ToArray());
		}
	}
}
