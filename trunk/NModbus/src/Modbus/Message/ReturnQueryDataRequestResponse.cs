using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Data;
using System.Net;

namespace Modbus.Message
{
	class ReturnQueryDataRequestResponse : ModbusMessageWithData<RegisterCollection>, IModbusMessage
	{
		private const int _minimumFrameSize = 4;

		public ReturnQueryDataRequestResponse()
		{
		}

		public ReturnQueryDataRequestResponse(byte slaveAddress, RegisterCollection data)
			: base(slaveAddress, Modbus.Diagnostics)
		{
			SubFunctionCode = Modbus.DiagnosticsReturnQueryData;
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
			//Data = new RegisterCollection(CollectionUtil.Slice<byte>(frame, 3, ByteCount));
		}
	}
}
