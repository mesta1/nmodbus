using System;
using System.Globalization;
using System.IO;
using Modbus.Data;
using Modbus.Utility;

namespace Modbus.Message
{
	class ReadWriteMultipleRegistersRequest : ModbusMessage, IModbusRequest
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
                byte[] read = CollectionUtility.Slice(_readRequest.ProtocolDataUnit, 1, _readRequest.ProtocolDataUnit.Length - 1);
                byte[] write = CollectionUtility.Slice(_writeRequest.ProtocolDataUnit, 1, _writeRequest.ProtocolDataUnit.Length - 1);

                return CollectionUtility.Concat(new byte[] { this.FunctionCode }, read, write);
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

		public override string ToString()
		{
			return String.Format(CultureInfo.InvariantCulture, "Write {0} holding registers starting at address {1}, and read {2} registers starting at address {3}.",
				_writeRequest.NumberOfPoints, _writeRequest.StartAddress, _readRequest.NumberOfPoints, _readRequest.StartAddress);
		}

        public void ValidateResponse(IModbusMessage response)
        {
            ReadHoldingInputRegistersResponse typedResponse = (ReadHoldingInputRegistersResponse) response;

            int expectedByteCount = ReadRequest.NumberOfPoints * 2;
            if (expectedByteCount != typedResponse.ByteCount)
            {
                throw new IOException(String.Format(CultureInfo.InvariantCulture,
                    "Unexpected byte count in response. Expected {0}, received {1}.", expectedByteCount, typedResponse.ByteCount));
            }
        }

		protected override void InitializeUnique(byte[] frame)
		{
			if (frame.Length < _minimumFrameSize + frame[10])
				throw new FormatException("Message frame does not contain enough bytes.");

			byte[] readFrame = CollectionUtility.Slice(frame, 2, 4);
			byte[] writeFrame = CollectionUtility.Slice(frame, 6, frame.Length - 6);
			byte[] header = { SlaveAddress, FunctionCode };

			_readRequest = ModbusMessageFactory.CreateModbusMessage<ReadHoldingInputRegistersRequest>(CollectionUtility.Concat(header, readFrame));
			_writeRequest = ModbusMessageFactory.CreateModbusMessage<WriteMultipleRegistersRequest>(CollectionUtility.Concat(header, writeFrame));
		}
    }
}
