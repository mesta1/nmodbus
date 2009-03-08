using MbUnit.Framework;
using Rhino.Mocks;
using Modbus.IO;
using System.IO.Ports;

namespace Modbus.UnitTests.IO
{
	[TestFixture]
	public class AbstractStreamResourceFixture
	{
		[Test]
		public void InitializeSerialPortTimeouts()
		{
			MockRepository mocks = new MockRepository();
			IStreamResource mockStreamResource = mocks.StrictMock<IStreamResource>();

			Expect.Call(mockStreamResource.WriteTimeout).Return(1);
			Expect.Call(mockStreamResource.InfiniteTimeout).Return(0);
			Expect.Call(mockStreamResource.WriteTimeout).Return(1);
			mockStreamResource.WriteTimeout = 1;
			Expect.Call(mockStreamResource.ReadTimeout).Return(1);
			Expect.Call(mockStreamResource.InfiniteTimeout).Return(0);
			Expect.Call(mockStreamResource.ReadTimeout).Return(1);
			mockStreamResource.ReadTimeout = 1;

			mocks.ReplayAll();
			StreamResourceUtility.InitializeDefaultTimeouts(mockStreamResource);
			mocks.VerifyAll();
		}

		[Test]
		public void SetupTimeoutsDefaultTimeout()
		{
			MockRepository mocks = new MockRepository();
			IStreamResource mockStreamResource = mocks.StrictMock<IStreamResource>();

			Expect.Call(mockStreamResource.WriteTimeout).Return(SerialPort.InfiniteTimeout);
			Expect.Call(mockStreamResource.InfiniteTimeout).Return(SerialPort.InfiniteTimeout);
			mockStreamResource.WriteTimeout = Modbus.DefaultTimeout;
			Expect.Call(mockStreamResource.ReadTimeout).Return(SerialPort.InfiniteTimeout);
			Expect.Call(mockStreamResource.InfiniteTimeout).Return(SerialPort.InfiniteTimeout);
			mockStreamResource.ReadTimeout = Modbus.DefaultTimeout;

			mocks.ReplayAll();
			StreamResourceUtility.InitializeDefaultTimeouts(mockStreamResource);
			mocks.VerifyAll();
		}
	}
}
