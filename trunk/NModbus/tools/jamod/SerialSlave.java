//License
/***
 * Java Modbus Library (jamod)
 * Copyright (c) 2002-2004, jamod development team
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are
 * met:
 *
 * Redistributions of source code must retain the above copyright notice,
 * this list of conditions and the following disclaimer.
 *
 * Redistributions in binary form must reproduce the above copyright notice,
 * this list of conditions and the following disclaimer in the documentation
 * and/or other materials provided with the distribution.
 *
 * Neither the name of the author nor the names of its contributors
 * may be used to endorse or promote products derived from this software
 * without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDER AND CONTRIBUTORS ``AS
 * IS'' AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
 * THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
 * PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE REGENTS OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 ***/

import net.wimpi.modbus.*;
import net.wimpi.modbus.net.*;
import net.wimpi.modbus.procimg.*;
import net.wimpi.modbus.util.*;

// class implementing a simple Modbus slave
public class SerialSlave
{

	public static void main(String[] args)
	{

		ModbusSerialListener listener = null;
		SimpleProcessImage spi = new SimpleProcessImage();
		String portname = args[0];

		if (Modbus.debug) System.out.println("jModbus ModbusSerial Slave");

		try
		{

			//1. Prepare a process image
			spi = new SimpleProcessImage();

			for (int i = 0; i < 600; i++)
				spi.addDigitalOut(new SimpleDigitalOut(false));


			for (int i = 0; i < 600; i++)
				spi.addDigitalIn(new SimpleDigitalIn(false));

			for (int i = 0; i < 600; i++)
				spi.addRegister(new SimpleRegister(0));

			for (int i = 0; i < 600; i++)
				spi.addInputRegister(new SimpleInputRegister(0));

			//2. Create the coupler and set the slave identity
			ModbusCoupler.getReference().setProcessImage(spi);
			ModbusCoupler.getReference().setMaster(false);
			ModbusCoupler.getReference().setUnitID(1);

			//3. Set up serial parameters
			SerialParameters params = new SerialParameters();
			params.setPortName(portname);
			params.setBaudRate(9600);
			params.setDatabits(8);
			params.setParity("None");
			params.setStopbits(1);
			params.setEncoding(args[1]);
			params.setEcho(false);
			if (Modbus.debug) System.out.println("Encoding [" + params.getEncoding() + "]");

			//4. Set up serial listener
			listener = new ModbusSerialListener(params);
			listener.setListening(true);

		}
		catch (Exception ex)
		{
			ex.printStackTrace();
		}
	}
}

