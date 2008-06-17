using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using log4net;
using Modbus.Message;
using Modbus.Utility;
using Unme.Common;
using System.Diagnostics;
using System.Reflection;

namespace Modbus.IO
{
	/// <summary>	
	/// Refined Abstraction - http://en.wikipedia.org/wiki/Bridge_Pattern
	/// </summary>
	internal class ModbusAsciiTransport : ModbusSerialTransport
	{
		private static readonly ILog _logger = LogManager.GetLogger(typeof(ModbusAsciiTransport));
		private Func<IStreamResource, Func<string>> _lineGetter;

		internal ModbusAsciiTransport(IStreamResource streamResource)
			: base(streamResource)
		{
			Debug.Assert(streamResource != null, "Argument streamResource cannot be null.");

			_lineGetter = FunctionalUtility.Memoize((IStreamResource stream) => GetReadLine());
		}		

		internal override byte[] BuildMessageFrame(IModbusMessage message)
		{
			List<byte> frame = new List<byte>();
			frame.Add((byte)':');
			frame.AddRange(ModbusUtility.GetAsciiBytes(message.SlaveAddress));
			frame.AddRange(ModbusUtility.GetAsciiBytes(message.ProtocolDataUnit));
			frame.AddRange(ModbusUtility.GetAsciiBytes(ModbusUtility.CalculateLrc(message.MessageFrame)));
			frame.AddRange(Encoding.ASCII.GetBytes(Modbus.NewLine.ToCharArray()));

			return frame.ToArray();
		}

		internal override bool ChecksumsMatch(IModbusMessage message, byte[] messageFrame)
		{
			return ModbusUtility.CalculateLrc(message.MessageFrame) == messageFrame[messageFrame.Length - 1];
		}

		internal override byte[] ReadRequest()
		{
			return ReadRequestResponse();
		}

		internal override IModbusMessage ReadResponse<T>()
		{
			return CreateResponse<T>(ReadRequestResponse());
		}

		/// <summary>
		/// HACK: Gets a Func&lt;string&gt; for reading a line. Use the ReadLine method on the IStreamResource
		/// if it exists (optimization), else use the naive implementation.
		/// </summary>
		internal virtual Func<string> GetReadLine()
		{
			var readLineMethod = StreamResource.GetType().GetMethod("ReadLine", BindingFlags.Public | BindingFlags.Instance);

			return readLineMethod == null && readLineMethod.ReturnType == typeof(string) ? 
				(Func<string>)(() => StreamResourceUtility.ReadLine(StreamResource)) :
				(Func<string>)(() => readLineMethod.Invoke(StreamResource, null) as string);
		}

		internal byte[] ReadRequestResponse()
		{
			// read message frame, removing frame start ':'
			string frameHex = _lineGetter(StreamResource).Invoke().Substring(1);

			// convert hex to bytes
			byte[] frame = ModbusUtility.HexToBytes(frameHex);
			_logger.InfoFormat("RX: {0}", frame.Join(", "));

			if (frame.Length < 3)
				throw new IOException("Premature end of stream, message truncated.");

			return frame;
		}
	}
}
