using System;
using System.IO;
using System.Windows;
using Renci.SshNet;
using System.Text.Json;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
        public MainWindow()
        {
            InitializeComponent();

            DirectoryInfo userDir = new DirectoryInfo(Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile));
            path = $"{userDir.FullName}\\.ssh_client_config.json";
            load_config();

            connect.Click += connect_ssh;
            disconnect.Click += disconnect_ssh;
            WindowState = WindowState.Minimized;
        }
        void load_config()
        {
            if (File.Exists(path))
            {
                File.ReadAllText(path);
                Config conf = JsonSerializer.Deserialize<Config>(File.ReadAllText(path));
                ip_address.Text = conf.ip_addr;
                port.Text = String.Concat(conf.port);
                username.Text = conf.username;
                password.Text = conf.password;
            }
        }
        void save_config()
        {
            String result = String.Empty;
            Config conf = new Config();
            conf.ip_addr = ip_address.Text;
            conf.port = Convert.ToInt32(port.Text);
            conf.username = username.Text;
            conf.password = password.Text;

            File.WriteAllText(path, JsonSerializer.Serialize<Config>(conf));
            
        }
        void connect_ssh(object sender, RoutedEventArgs e)
        {
            client = new SshClient(ip_address.Text, int.Parse(port.Text), username.Text, password.Text);
            client.Connect();
            if (client.IsConnected)
            {
                save_config();
            }
            var portForward = new ForwardedPortDynamic("127.0.0.1", 6030);
            client.AddForwardedPort(portForward);
            portForward.Start();

        }
        void disconnect_ssh(object sender, RoutedEventArgs e)
        {
            if (client != null && client.IsConnected)
            {
                client.Disconnect();
            }
        }
    }
}
