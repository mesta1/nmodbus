//using System;
//using Modbus.Message;

//namespace Modbus.IO
//{
//    class ModbusUsbTransport : ModbusTransport
//    {
//        /// <summary>
//        /// Initializes a new instance of the <see cref="ModbusUsbTransport"/> class.
//        /// </summary>
//        public ModbusUsbTransport()
//        {
//        }

//        public ModbusUsbTransport(UsbAdapter stream)
//        {

//        }

//        internal override byte[] ReadRequest()
//        {
//            throw new Exception("The method or operation is not implemented.");
//        }

//        internal override IModbusMessage ReadResponse<T>()
//        {
//            throw new Exception("The method or operation is not implemented.");
//        }

//        internal override byte[] BuildMessageFrame(IModbusMessage message)
//        {
//            throw new Exception("The method or operation is not implemented.");
//        }

//        internal override void Write(IModbusMessage message)
//        {
//            throw new Exception("The method or operation is not implemented.");
//        }
//    }
//}
