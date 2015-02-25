using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace UDP_Server_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] data = new byte[1024];
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 515);
            UdpClient newsock = null;
            int count = 0;
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);

            while (1 == 1)
            {
                try
                {
                    newsock = new UdpClient(ipep);

                    //Console.WriteLine("Waiting for a client...");

                    data = newsock.Receive(ref sender);

                    //Console.WriteLine("Message received from {0}:", sender.ToString());
                    //Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));
                    newsock.Close();
                    count++;
                }
                catch (Exception)
                {
                    //throw;
                }
                Console.WriteLine(count);
            }

        }
    }
}
