using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Util;
using Modbus.Data;

namespace Modbus.Message
{
	class ReadWriteMultipleRegistersRequest : ModbusMessage, IModbusMessage
	{
		private const int _minimumFrameSize = 11;
		private ReadHoldingInputRegistersRequest _readRequest;
		private WriteMultipleRegistersRequest _writeRequest;

		public ReadWriteMultipleRegistersRequest()
		{
		}

		public ReadWriteMultipleRegistersRequest(byte slaveAddress, ushort startReadAddress, ushort numberOfPointsToRead, ushort startWriteAddress, RegisterCollection writeData)
			: base(slaveAddress, Modbus.ReadWriteMultipleRegisters)
		{
			_readRequest = new ReadHoldingInputRegistersRequest(Modbus.ReadHoldingRegisters, slaveAddress, startReadAddress, numberOfPointsToRead);
			_writeRequest = new WriteMultipleRegistersRequest(slaveAddress, startWriteAddress, writeData);
		}

		public override byte[] ProtocolDataUnit
		{
			get
			{
				// read and write PDUs without function codes
				byte[] read = CollectionUtil.Slice(_readRequest.ProtocolDataUnit, 1, _readRequest.ProtocolDataUnit.Length - 1);
				byte[] write = CollectionUtil.Slice(_writeRequest.ProtocolDataUnit, 1, _writeRequest.ProtocolDataUnit.Length - 1);

				return CollectionUtil.Combine(new byte[] { this.FunctionCode }, read, write);
			}
		}

		public ReadHoldingInputRegistersRequest ReadRequest
		{					
			get { return _readRequest; }
		}

		public WriteMultipleRegistersRequest WriteRequest
		{
			get { return _writeRequest; }
		}
	
		public override int MinimumFrameSize
		{
			get { return _minimumFrameSize; }
		}

		protected override void InitializeUnique(byte[] frame)
		{
			if (frame.Length < _minimumFrameSize + frame[10])
				throw new FormatException("Message frame does not contain enough bytes.");

			byte[] readFrame = CollectionUtil.Slice(frame, 2, 4);
			byte[] writeFrame = CollectionUtil.Slice(frame, 6, frame.Length - 6);
			byte[] header = { SlaveAddress, FunctionCode };

			_readRequest = ModbusMessageFactory.CreateModbusMessage<ReadHoldingInputRegistersRequest>(CollectionUtil.Combine(header, readFrame));
			_writeRequest = ModbusMessageFactory.CreateModbusMessage<WriteMultipleRegistersRequest>(CollectionUtil.Combine(header, writeFrame));
		}
	}
}
