using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static bool running = true;
        static Task task = null;
        static void Main(string[] args)
        {
            Console.WriteLine("Start Server");

            task = Task.Run(() => RunServer());

            while (running)
            {
                System.Threading.Thread.Sleep(1);
            }
        }
        static async void RunServer()
        {
            var ip = IPAddress.Parse("192.168.10.100");
            var port = 1000;

            var tcpListener = new TcpListener(ip, port);

            tcpListener.Start();

            while (true)
            {


                string line;
                do
                {
                    using (var tcpClient = await tcpListener.AcceptTcpClientAsync())
                    using (var stream = tcpClient.GetStream())
                    using (var reader = new StreamReader(stream))
                    using (var writer = new StreamWriter(stream))
                    {
                        // 接続元
                        Console.WriteLine(tcpClient.Client.RemoteEndPoint);

                        line = await reader.ReadLineAsync();

                        // 受信メッセージ
                        if (line != null && line != "")
                        {
                            Console.WriteLine($"recieve message:{line}");
                        };
                    }

                } while (true);

                // 返信
                //await writer.WriteLineAsync("Server has recieved your message");
                //await writer.WriteLineAsync(); // 終わり


                //Console.WriteLine("end connection");
            }

            running = false;
        }
    }
}
