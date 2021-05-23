using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerForOtogameController
{
    public partial class Form1 : Form
    {
        private TcpListener tcplistener = null;
        private TcpClient tcpClient = null;
        private NetworkStream networkStream = null;
        private StreamReader streamReader = null;
        //private StreamWriter streamWriter = null;

        private int port = 9999;
        private string serverIP = "127.0.0.1";
        public Form1()
        {
            InitializeComponent();

            //ActionInfo.Insert(0, DateTime.Now.ToString("hh:mm:ss") + "," + methodName + "," + "接続待機");
            tcplistener = new TcpListener(IPAddress.Any, port);

            tcplistener.Start();
            Task.Run(async () =>
            {
                Console.WriteLine("クライアント接続を待機中...");
                //クライアントの要求があったら、接続を確立する(接続があるかtcplistener.Stop()が実行されるまで待機する)
                await Task.Run(() => tcpClient = tcplistener.AcceptTcpClient());
                
                networkStream = tcpClient.GetStream();
                streamReader = new StreamReader(networkStream, Encoding.UTF8);
                //streamWriter = new StreamWriter(networkStream, Encoding.UTF8);
                Task.Run(() => Receive());
            });

            //テストクライアント
            Task.Run(async () =>
            {

                var port = 9999;
                var ip = "127.0.0.1";
                var client = new TcpClient();
            });

        }

        /// <summary>
        /// 信号受信(受信があるまで待機する)
        /// </summary>
        private async void Receive()
        {
            try
            {
                await Task.Run(() =>
                {
                    while (true)
                    {
                        string res = null;
                        byte[] data = new byte[256];
                        string receiveData = string.Empty;
                        int bytes = networkStream.Read(data, 0, data.Length);
                        if (bytes != 0)
                        {
                            res = Encoding.UTF8.GetString(data, 0, bytes);
                            Console.WriteLine(res);
                        }

                        Task.Delay(1);
                        
                    }
                    
                    tcplistener.Stop();
                });
            }
            catch (Exception ex)
            {
            }
        }

    }
}
