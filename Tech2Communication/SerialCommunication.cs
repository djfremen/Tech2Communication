using System;
using System.IO.Ports;
using System.Threading;

namespace Tech2Communication
{
    public class SerialCommunication
    {
        private SerialPort serialPort;
        private readonly object syncLock = new object();

        public SerialCommunication(string portName, int baudRate, Parity parity, StopBits stopBits)
        {
            serialPort = new SerialPort
            {
                PortName = portName,
                BaudRate = baudRate,
                Parity = parity,
                DataBits = 8,
                StopBits = stopBits,
                ReadTimeout = 1000,
                WriteTimeout = 1000,
                ReceivedBytesThreshold = 1
            };
        }

        public bool IsOpen => serialPort != null && serialPort.IsOpen;

        public void Open()
        {
            if (serialPort != null && !serialPort.IsOpen)
            {
                serialPort.Open();
            }
        }

        public void Close()
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }

        public void Send(byte[] data)
        {
            if (!IsOpen)
                throw new InvalidOperationException("Serial port is not open");

            lock (syncLock)
            {
                serialPort.DiscardInBuffer();
                serialPort.Write(data, 0, data.Length);

                // Log for debugging
                Console.WriteLine("TX: " + BitConverter.ToString(data));
            }
        }

        public byte[] Receive(int timeout)
        {
            if (!IsOpen)
                throw new InvalidOperationException("Serial port is not open");

            byte[] buffer = new byte[1024];
            int bytesRead = 0;

            lock (syncLock)
            {
                // Set a timeout for reading
                DateTime startTime = DateTime.Now;

                while ((DateTime.Now - startTime).TotalMilliseconds < timeout)
                {
                    if (serialPort.BytesToRead > 0)
                    {
                        // Read whatever is available
                        int newBytes = serialPort.Read(buffer, bytesRead, Math.Min(serialPort.BytesToRead, buffer.Length - bytesRead));
                        bytesRead += newBytes;

                        // Wait a bit for any additional data
                        Thread.Sleep(50);

                        // If no more data for a while, assume the response is complete
                        if (serialPort.BytesToRead == 0)
                            break;
                    }

                    Thread.Sleep(10);
                }
            }

            if (bytesRead > 0)
            {
                // Create response array of exact size
                byte[] response = new byte[bytesRead];
                Array.Copy(buffer, response, bytesRead);

                // Log for debugging
                Console.WriteLine("RX: " + BitConverter.ToString(response));
                return response;
            }

            return null;
        }

        public byte[] SendAndReceive(byte[] data, int timeout)
        {
            Send(data);
            return Receive(timeout);
        }

        // Helper method for formatting byte arrays for diagnostic output
        public static string FormatByteArray(byte[] data)
        {
            if (data == null || data.Length == 0)
                return "[]";

            return BitConverter.ToString(data).Replace("-", " ");
        }
    }
}