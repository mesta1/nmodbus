using System.IO;
using Modbus.Data;
using Modbus.IO;
using Modbus.Message;
using Modbus.UnitTests.Message;
using Modbus.Util;
using NUnit.Framework;

namespace Modbus.UnitTests.IO
{
	[TestFixture]
	public class ModbusSerialTransportFixture : ModbusMessageFixture
	{
		[Test]
		public void CreateResponse()
		{
			ModbusAsciiTransport transport = new ModbusAsciiTransport();
			ReadCoilsInputsResponse expectedResponse = new ReadCoilsInputsResponse(Modbus.ReadCoils, 2, 1, new DiscreteCollection(true, false, false, false, false, false, false, true));
			byte lrc = ModbusUtility.CalculateLrc(expectedResponse.MessageFrame);
			ReadCoilsInputsResponse response = transport.CreateResponse<ReadCoilsInputsResponse>(new byte[] { 2, Modbus.ReadCoils, 1, 129, lrc }) as ReadCoilsInputsResponse;
			Assert.IsNotNull(response);
			AssertModbusMessagePropertiesAreEqual(expectedResponse, response);
		}

		[Test, ExpectedException(typeof(IOException))]
		public void CreateResponseErroneousLrc()
		{
			ModbusAsciiTransport transport = new ModbusAsciiTransport();
			transport.CheckFrame = true;
			transport.CreateResponse<ReadCoilsInputsResponse>(new byte[] { 19, Modbus.ReadCoils, 0, 0, 0, 2, 115 });
		}

		[Test]
		public void CreateResponseErroneousLrcDoNotCheckFrame()
		{
			ModbusAsciiTransport transport = new ModbusAsciiTransport();
			transport.CheckFrame = false;
			transport.CreateResponse<ReadCoilsInputsResponse>(new byte[] { 19, Modbus.ReadCoils, 0, 0, 0, 2, 115 });
		}
	}
}
