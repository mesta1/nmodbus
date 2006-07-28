using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Data;

namespace Modbus.Message
{
	public abstract class ModbusMessage
	{
		private ModbusMessageImpl _messageImpl;

		public ModbusMessage()
		{
			_messageImpl = new ModbusMessageImpl();
		}

		public ModbusMessage(byte slaveAddress, byte functionCode)
		{
			_messageImpl = new ModbusMessageImpl(slaveAddress, functionCode);
		}

		public byte FunctionCode
		{
			get { return _messageImpl.FunctionCode; }
			set { _messageImpl.FunctionCode = value; }
		}

		public byte SlaveAddress
		{
			get { return _messageImpl.SlaveAddress; }
			set { _messageImpl.SlaveAddress = value; }
		}

		protected ModbusMessageImpl MessageImpl
		{
			get { return _messageImpl; }
		}

		public byte[] ProtocolDataUnit
		{
			get { return _messageImpl.ProtocolDataUnit; }
		}

		public byte[] ChecksumBody
		{
			get { return _messageImpl.ChecksumBody; }
		}

		public void InitializeCommon(byte[] frame)
		{
			_messageImpl.Initialize(frame);
			InitializeUnique(frame);
		}

		protected abstract void InitializeUnique(byte[] frame);
	}
}
