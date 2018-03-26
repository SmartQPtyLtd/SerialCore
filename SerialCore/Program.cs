﻿using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace SerialCore
{
    class Program
    {
        static SerialPort Port;
        static int Baudrate = 9600;

        static List<string> Ports = new List<string>();
        static List<int> Baudrates = new List<int>
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

        static SerialPort Read(string port, int baudrate)
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

            try { Port.Open(); }
            catch { Console.WriteLine("Port Already Open!"); }

            return Port;
        }

        static void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort port = (SerialPort)sender;
            Console.Write(port.ReadExisting());
        }

        static void ListSerialPorts()
        {
            Console.Write("Available Serial ports:");
            foreach (var port in SerialPort.GetPortNames())
            {
                Ports.Add(port);
                Console.Write(" [" + port + ']');
            }

            Console.WriteLine();
        }

        static void SetBaudRate()
        {
            string input;

            while (true)
            {
                Console.Write("Set Baudrate (Optional) : ");
                input = Console.ReadLine();

                if (string.IsNullOrEmpty(input.Trim()))
                {
                    Console.WriteLine($"Baudrate: {Baudrate} [Default]");
                    break;
                }

                if (int.TryParse(input, out Baudrate))
                {
                    if (Baudrates.Contains(Baudrate))
                        break;
                }
            }
        }

        static void SendCommands()
        {
            string commands;
            while (true)
            {
                commands = Console.ReadLine() + '\r';
                if (commands.ToUpper().StartsWith("EXIT"))
                {
                    if (Port != null)
                        Port.Close();

                    break;
                }

                Port.WriteLine(commands);
            }
        }

        static void SetPort()
        {
            string port;

            while (true)
            {
                Console.Write("Please Enter Port: ");
                port = Console.ReadLine().ToUpper();
                if (Ports.Contains(port))
                    break;

            }

            Console.WriteLine("Ready:");
            Port = Read(port, Baudrate);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("SerialCore v1.0 Serial Communications For .Net Core 2.0\r\nJawid Hassim, adapted from Jeremy Lindsay.");

            ListSerialPorts();

            if (Ports.Count != 0)
            {
                SetBaudRate();

                if (Ports.Count == 1)
                {
                    Console.WriteLine("Ready:");
                    Port = Read(Ports[0], Baudrate);
                }
                else SetPort();

                SendCommands();
                Ports.Clear();
            }
            else
            {
                Console.WriteLine("No Serial Ports Found! Bye");
                Console.ReadKey();
            }

            if (Port != null)
                Port.Close();

            Baudrates.Clear();
            Port.Dispose();
            Environment.Exit(0);
        }
    }
}