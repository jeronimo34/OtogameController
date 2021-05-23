using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Android.App;
using Android.Hardware.Usb;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
//using Android.Hardware.Usb;
using Xamarin.Android.Net;

namespace OtogameController
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            //Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            //SetSupportActionBar(toolbar);

            //FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            //fab.Click += FabOnClick;

            var button1 = FindViewById<Button>(Resource.Id.button1);

            button1.Click += FabOnClickAsync;

            var button2 = FindViewById<Button>(Resource.Id.button2);
            button2.Click += FabOnClickAsync;


        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private async void FabOnClickAsync(object sender, EventArgs eventArgs)
        {
            try {
                View view = (View)sender;
                Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                    .SetAction("Action", (View.IOnClickListener)null).Show();

                string ip = "192.168.10.100";// "127.0.0.1";
                int port = 1000;

                using (var tcpClient = new TcpClient(ip, port))
                using (var stream = tcpClient.GetStream())
                using (var writer = new StreamWriter(stream))
                {
                    Console.WriteLine("サーバー({0}:{1})と接続しました({2}:{3})。",
                    ((System.Net.IPEndPoint)tcpClient.Client.RemoteEndPoint).Address,
                    ((System.Net.IPEndPoint)tcpClient.Client.RemoteEndPoint).Port,
                    ((System.Net.IPEndPoint)tcpClient.Client.LocalEndPoint).Address,
                    ((System.Net.IPEndPoint)tcpClient.Client.LocalEndPoint).Port);

                    var msg = "hoge";
                    await writer.WriteLineAsync(msg);
                    await writer.WriteLineAsync();
                    await writer.FlushAsync();
                }

                Console.WriteLine("end connection");

                var button1 = FindViewById<Button>(Resource.Id.button1);
                UsbManager manager = (UsbManager)GetSystemService(UsbService);

                button1.Text += "hgoee";
                Console.WriteLine(button1.Text);
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
            
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

