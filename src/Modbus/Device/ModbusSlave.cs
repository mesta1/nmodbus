using System;
using System.Collections.Generic;
using System.Text;
using Modbus.IO;
using Modbus.Message;

namespace Modbus.Device
{
	public abstract class ModbusSlave : ModbusDevice
	{
		private byte _unitID;

		public byte UnitID
		{
			get { return _unitID; }
			set { _unitID = value; }
		}

		internal ModbusSlave(byte unitID, ModbusTransport transport)
			: base(transport)
		{
			_unitID = unitID;
		}

		public void Listen()
		{
			throw new Exception("The method or operation is not implemented.");
			//while (true)
			//{
			//    // read request
			//    ModbusRequest request = Transport.ReadRequest();

			//    // create response
			//    ModbusResponse response = null; // new ModbusResponse(_unitID, request.FunctionCode, request.RequestCount);

			//    // write response
			//    Transport.WriteMessage(response);
			//}
		}
	}
}
