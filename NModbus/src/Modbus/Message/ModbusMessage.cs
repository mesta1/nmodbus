using System;

namespace Modbus.Message
{
	abstract class ModbusMessage
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

		public ushort TransactionID
		{
			get { return _messageImpl.TransactionID; }
			set { _messageImpl.TransactionID = value; }
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

		public ModbusMessageImpl MessageImpl
		{
			get { return _messageImpl; }
		}

		public byte[] MessageFrame
		{
			get { return _messageImpl.MessageFrame; }
		}

		public virtual byte[] ProtocolDataUnit
		{
			get { return _messageImpl.ProtocolDataUnit; }
		}

		public void Initialize(byte[] frame)
		{
			if (frame.Length < MinimumFrameSize)
				throw new FormatException(String.Format("Message frame must contain at least {0} bytes of data.", MinimumFrameSize));

			_messageImpl.Initialize(frame);
			InitializeUnique(frame);
		}

		protected abstract void InitializeUnique(byte[] frame);
		public abstract int MinimumFrameSize { get; }
	}
}
