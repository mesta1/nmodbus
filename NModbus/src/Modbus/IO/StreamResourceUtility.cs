using System.Linq;
using System.Text;

namespace Modbus.IO
{
	internal static class StreamResourceUtility
	{
		internal static string ReadLine(IStreamResource stream)
		{
			var result = new StringBuilder();
			var singleByteBuffer = new byte[1];

			do
			{
				stream.Read(singleByteBuffer, 0, 1);
				result.Append(Encoding.ASCII.GetChars(singleByteBuffer).First());
			} while (!result.ToString().EndsWith(Modbus.NewLine));

			return result.ToString().Substring(0, result.Length - Modbus.NewLine.Length);
		}

		/// <summary>
		/// Initializes stream read write timeouts to default value if they have not been overridden already.
		/// </summary>
		internal static void InitializeDefaultTimeouts(IStreamResource streamResource)
		{
#if !WindowsCE
			streamResource.WriteTimeout = streamResource.WriteTimeout == streamResource.InfiniteTimeout ? Modbus.DefaultTimeout : streamResource.WriteTimeout;
			streamResource.ReadTimeout = streamResource.ReadTimeout == streamResource.InfiniteTimeout ? Modbus.DefaultTimeout : streamResource.ReadTimeout;
#endif
		}
	}
}
