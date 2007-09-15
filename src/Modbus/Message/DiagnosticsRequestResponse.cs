using System;
using System.Net;
using Modbus.Data;
using Modbus.Utility;

namespace Modbus.Message
{
	class DiagnosticsRequestResponse : ModbusMessageWithData<RegisterCollection>, IModbusMessage
	{
		private const int _minimumFrameSize = 6;

		public DiagnosticsRequestResponse()
		{
		}

		public DiagnosticsRequestResponse(ushort subFunctionCode, byte slaveAddress, RegisterCollection data)
			: base(slaveAddress, Modbus.Diagnostics)
		{
			SubFunctionCode = subFunctionCode;
			Data = data;
		}
		
		public override int MinimumFrameSize
		{
			get { return _minimumFrameSize; }
		}

		public ushort SubFunctionCode
		{
			get { return MessageImpl.SubFunctionCode; }
			set { MessageImpl.SubFunctionCode = value; }
		}

		protected override void InitializeUnique(byte[] frame)
		{
			SubFunctionCode = (ushort) IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 2));
			Data = new RegisterCollection(CollectionUtility.Slice<byte>(frame, 4, 2));
		}
	}
}
