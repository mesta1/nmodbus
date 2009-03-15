using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using Modbus.Data;
using Unme.Common;

namespace Modbus.Message
{
	internal class DiagnosticsRequestResponse : ModbusMessageWithData<RegisterCollection>, IModbusMessage
	{		
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
			get { return 6; }
		}

		public ushort SubFunctionCode
		{
			get { return MessageImpl.SubFunctionCode; }
			set { MessageImpl.SubFunctionCode = value; }
		}

		public override string ToString()
		{
			Debug.Assert(SubFunctionCode == Modbus.DiagnosticsReturnQueryData, "Need to add support for additional sub-function.");

			return String.Format(CultureInfo.InvariantCulture, "Diagnostics message, sub-function return query data - {0}.", Data);
		}

		protected override void InitializeUnique(byte[] frame)
		{
			SubFunctionCode = (ushort) IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 2));
			Data = new RegisterCollection(frame.Slice(4, 2).ToArray());
		}
	}
}
