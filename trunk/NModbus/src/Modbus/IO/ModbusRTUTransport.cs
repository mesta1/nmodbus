using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Message;
using System.IO.Ports;
using Modbus.Util;
using System.IO;

namespace Modbus.IO
{
	public class ModbusRTUTransport : ModbusSerialTransport, IModbusTransport
	{
		public ModbusRTUTransport(SerialPort serialPort)
			: base (serialPort)
		{
		}
	
		// TODO common code
		// TOOD use frame word here
		public override void Write(IModbusMessage message)
		{
			List<byte> messageBody = new List<byte>();
			messageBody.Add(message.SlaveAddress);
			messageBody.AddRange(message.ProtocolDataUnit);
			messageBody.AddRange(CalculateChecksum(message));

			SerialPort.Write(messageBody.ToArray(), 0, messageBody.Count);
		}

		public override T Read<T>(IModbusMessage request)
		{
			throw new Exception("The method or operation is not implemented.");
			//try
			//{
			//    byte unitID = (byte)SerialPort.ReadByte();
			//    byte functionCode = (byte)SerialPort.ReadByte();
			//    byte count = (byte)SerialPort.ReadByte();
			//    byte[] data = new byte[count];
			//    SerialPort.Read(data, 0, data.Length);

			//    byte[] crc = new byte[2];
			//    SerialPort.Read(crc, 0, crc.Length);

			//    ModbusResponse response = new ModbusResponse(unitID, functionCode, count);
			//    response.SetData(data, dataLength);

			//    // check LRC
			//    byte[] checksum = CalculateChecksum(response);
			//    if (checksum[0] != crc[0] || checksum[1] != crc[1])
			//    {
			//        _log.ErrorFormat("Checksum error - calculated value {0} {1} != received value {2} {3}", checksum[0], checksum[1], crc[0], crc[1]);
			//        throw new IOException("Checksum LRC failed");
			//    }

			//    return response;
			//}
			//catch (TimeoutException te)
			//{
			//    throw te;
			//}
			//catch (IOException ioe)
			//{

			//    throw ioe;
			//}
		}

		public IModbusMessage ReadResponse(string message, ushort dataLength)
		{
			return null;
		}

		/// <summary>
		/// Cyclic Redundancy Check
		/// </summary>
		public override byte[] CalculateChecksum(IModbusMessage messsage)
		{
			return ModbusUtil.CalculateCRC(messsage.ChecksumBody);
		}
	}
}
