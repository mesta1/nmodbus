using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using Modbus.Data;

namespace Modbus.Message
{
	class WriteSingleRegisterRequestResponse : ModbusMessageWithData<RegisterCollection>, IModbusRequest
	{
		private const int _minimumFrameSize = 6;

		public WriteSingleRegisterRequestResponse()
		{
		}

		public WriteSingleRegisterRequestResponse(byte slaveAddress, ushort startAddress, ushort registerValue)
			: base(slaveAddress, Modbus.WriteSingleRegister)
		{
			StartAddress = startAddress;
			Data = new RegisterCollection(registerValue);
		}

		public override int MinimumFrameSize
		{
			get { return _minimumFrameSize; }
		}

		public ushort StartAddress
		{
			get { return MessageImpl.StartAddress; }
			set { MessageImpl.StartAddress = value; }
		}

		public override string ToString()
		{
			return String.Format(CultureInfo.InvariantCulture, "Write single holding register at address {0}.", StartAddress);
		}

        public void ValidateResponse(IModbusMessage response)
        {
            var typedResponse = (WriteSingleCoilRequestResponse) response;

            if (StartAddress != typedResponse.StartAddress)
            {
                throw new IOException(String.Format(CultureInfo.InvariantCulture,
                    "Unexpected start address in response. Expected {0}, received {1}.", StartAddress, typedResponse.StartAddress));
            }

            if (Data.First() != typedResponse.Data.First())
            {
                throw new IOException(String.Format(CultureInfo.InvariantCulture,
                    "Unexpected data in response. Expected {0}, received {1}.", Data.First(), typedResponse.Data.First()));
            }
        }

		protected override void InitializeUnique(byte[] frame)
		{
			StartAddress = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 2));
			Data = new RegisterCollection((ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 4)));
		}
	}
}
