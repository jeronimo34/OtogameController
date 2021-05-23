using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static bool running = true;
        static Task task = null;
        static void Main(string[] args)
        {
            Console.WriteLine("Run Client");

            task = new Task(() => RunClient());
            task.Start();

            while (running) {
                System.Threading.Thread.Sleep(1);
            }
        }

        public static async void RunClient()
        {
            

            string ip = "192.168.10.100";
            int port = 1000;

            using (var tcpClient = new TcpClient(ip, port))
            using (var stream = tcpClient.GetStream())
            using (var reader = new StreamReader(stream))
            using (var writer = new StreamWriter(stream))
            {
                Console.WriteLine("サーバー({0}:{1})と接続しました({2}:{3})。",
                ((System.Net.IPEndPoint)tcpClient.Client.RemoteEndPoint).Address,
                ((System.Net.IPEndPoint)tcpClient.Client.RemoteEndPoint).Port,
                ((System.Net.IPEndPoint)tcpClient.Client.LocalEndPoint).Address,
                ((System.Net.IPEndPoint)tcpClient.Client.LocalEndPoint).Port);

                while (true)
                {
                    var msg = Console.ReadLine();
                    if (msg == "END") break;
                    await writer.WriteLineAsync(msg);
                    await writer.WriteLineAsync();
                    await writer.FlushAsync();
                }
                /*
                string line;
                do
                {
                    line = await reader.ReadLineAsync();
                    // 受信メッセージ
                    if (line != "") Console.WriteLine($"message form Server:{line}");

                } while (!String.IsNullOrWhiteSpace(line));*/
            }

            Console.WriteLine("end connection");
            running = false;
        }
    }
}
