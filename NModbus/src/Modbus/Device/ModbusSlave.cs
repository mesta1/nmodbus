using System;
using System.Collections.Generic;
using System.Text;
using Modbus.IO;
using Modbus.Message;

namespace Modbus.Device
{
	internal abstract class ModbusSlave : ModbusDevice
	{
		private byte _unitID;

		public ModbusSlave(byte unitID, ModbusTransport transport)
			: base(transport)
		{
			_unitID = unitID;
		}

		public byte UnitID
		{
			get { return _unitID; }
			set { _unitID = value; }
		}
	
		public void Listen()
		{
			while (true)
			{
				// use transport to retrieve raw message frame from stream
				byte[] frame = Transport.GetMessageFrame();

				// build request from frame
				IModbusMessage request = ModbusMessageFactory.CreateModbusRequest(frame);

				// build response from request
				// perform action

				// request.CreateResponse();
				// send request somewhere else...



				// write response
			}
		}
	}
}
