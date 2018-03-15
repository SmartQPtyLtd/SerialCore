using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace SerialCore
{
    class Program
    {
        public static List<int> Baudrates = new List<int>
        {
            110,
            300,
            600,
            1200,
            2400,
            4800,
            9600,
            14400,
            19200,
            28800,
            38400,
            56000,
            57600,
            115200,
            128000,
            153600,
            230400,
            256000,
            460800,
            921600
        };

        public static List<string> Ports = new List<string>();

        public static SerialPort Read(string port, int baudrate)
        {
            SerialPort Port = new SerialPort(port)
            {
                BaudRate = baudrate,
                Parity = Parity.None,
                StopBits = StopBits.One,
                DataBits = 8,
                Handshake = Handshake.None
            };

            Port.DataReceived += SerialPortDataReceived;
            Port.Open();

            return Port;
        }

        private static void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort port = (SerialPort)sender;
            Console.Write(port.ReadExisting());
        }

        public static void SerialPorts()
        {
            Console.Write("Available Serial ports: ");
            foreach (var port in SerialPort.GetPortNames())
            {
                Ports.Add(port);
                Console.Write(port);
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("SerialCore v1.0 Serial Reader For .Net Core 2.0\r\nJawid Hassim, adapted from Jeremy Lindsay.");

            SerialPorts();
            Console.WriteLine();

            SerialPort Port = null;

            if (Ports.Count != 0)
            {
                int baudrate = 9600;
                string input;

                while (true)
                {
                    Console.Write("Set Baudrate: ");
                    input = Console.ReadLine();

                    if (string.IsNullOrEmpty(input))
                    {
                        Console.WriteLine("Baudrate: 9600 [Default]");
                        break;
                    }

                    if (int.TryParse(input, out baudrate))
                    {
                        if (Baudrates.Contains(baudrate))
                            break;
                    }
                }

                if (Ports.Count == 1)
                {
                    Console.Write("Reading Data: ");
                    Port = Read(Ports[0], baudrate);
                }
                else
                {
                    string port;

                    while (true)
                    {
                        Console.Write("Please Enter Port: ");
                        port = Console.ReadLine();
                        if (Ports.Contains(port))
                            break;
                    }

                    Console.WriteLine("Reading Data:");
                    Port = Read(port, baudrate);
                }
            }
            else Console.WriteLine("No Serial Ports Found! Bye");
            Console.ReadKey();

            if (Port != null)
                Port.Close();

            Environment.Exit(0);
        }
    }
}