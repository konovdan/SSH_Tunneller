using System.Windows;
using BackgroundServiceRPC;
using Grpc.Core;
using Grpc.Net.Client;
using Renci.SshNet;

namespace WpfApp1
{
    public class Config
    {
        public string ip_addr { get; set; }
        public int port { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
    public partial class MainWindow : Window
    {
        private SshClient client;
        private string path;
        private BackgroundService.BackgroundServiceClient grpc_client;
        public MainWindow()
        {
            InitializeComponent();

            grpc_client = new BackgroundService.BackgroundServiceClient(GrpcChannel.ForAddress("http://localhost:5000"));
            load_config();

            connect.Click += connect_ssh;
            disconnect.Click += disconnect_ssh;
            WindowState = WindowState.Minimized;
        }
        void load_config()
        {
            try
            {
                BackgroundServiceRPC.Config config = grpc_client.GetConfig(new Empty { });
                ip_address.Text = config.IpAddress;
                port.Text = String.Concat(config.Port);
                username.Text = config.Username;
                password.Text = config.Password;
            }
            catch (RpcException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        void save_config()
        {
            grpc_client.SetConfig(new BackgroundServiceRPC.Config
            {
                IpAddress = ip_address.Text,
                Port = port.Text,
                Username = username.Text,
                Password = password.Text
            });
        }
        void connect_ssh(object sender, RoutedEventArgs e)
        {
            try
            {
                grpc_client.ConnectSSH(new Empty { });
            }
            catch (RpcException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void disconnect_ssh(object sender, RoutedEventArgs e)
        {
            try
            {
                grpc_client.DisconnectSSH(new Empty { });
            }
            catch (RpcException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
