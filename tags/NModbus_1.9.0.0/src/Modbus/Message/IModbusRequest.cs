using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modbus.Message
{
    /// <summary>
    /// Methods specific to a modbus request message.
    /// </summary>
    public interface IModbusRequest : IModbusMessage
    {
        /// <summary>
        /// Validate the specified response against the current request.
        /// </summary>
        void ValidateResponse(IModbusMessage response);
    }
}
