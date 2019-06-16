﻿using System;
using System.Net;
using System.Text;
using System.Net.Sockets;


namespace ftpClient
{
    class ControlSocket
    {
        private static Byte[] buffer = new Byte[1024 * 1024 * 2];
        private int bufferLength;
        private Socket socket;
        private String receiveMessage()
        {
            bufferLength = socket.Receive(buffer);
            String message = Encoding.UTF8.GetString(buffer, 0, bufferLength);
            Console.WriteLine(message);
            return message;
        }
        public ControlSocket(IPAddress serverIp, int serverPort)
        {
            IPEndPoint ipe = new IPEndPoint(serverIp, 12345);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ipe);
            receiveMessage();
        }
        public void USER(String user)
        {
            socket.Send(Encoding.UTF8.GetBytes($"USER {user}\r\n"));
            receiveMessage();
        }
        public void PASS(String pass)
        {
            socket.Send(Encoding.UTF8.GetBytes($"PASS {pass}\r\n"));
            receiveMessage();
        }
        public int SIZE(String fname)
        {
            socket.Send(Encoding.UTF8.GetBytes($"SIZE {fname}\r\n"));
            String message = receiveMessage();
            int st = message.IndexOf(' ') + 1;
            int ed = message.Length - 1;
            return int.Parse(message.Substring(st, ed - st));
        }
        public void PWD()
        {
            socket.Send(Encoding.UTF8.GetBytes("PWD\r\n"));
            receiveMessage();
        }
        public int PASV()
        {
            socket.Send(Encoding.UTF8.GetBytes("PASV\r\n"));
            String message = receiveMessage();
            int st = message.IndexOf('(') + 1;
            int ed = message.IndexOf(')');
            String[] number = message.Substring(st, ed - st).Split(new char[] { ',' });
            return int.Parse(number[4]) * 256 + int.Parse(number[5]);
        }
        public void RETR(string fname)
        {
            socket.Send(Encoding.UTF8.GetBytes($"RETR {fname}\r\n"));
            receiveMessage();
        }
        public void STOR(string fname)
        {
            socket.Send(Encoding.UTF8.GetBytes($"STOR {fname}\r\n"));
            receiveMessage();
        }
        public void DATA_END()
        {
            receiveMessage();
        }
        public void LIST(string fname)
        {
            socket.Send(Encoding.UTF8.GetBytes($"LIST {fname}\r\n"));
            receiveMessage();
        }
    }
}
