using System;
using System.IO;
using System.Windows;
using Renci.SshNet;
using System.Text.Json;

namespace Background_service
{
    public class Config
    {
        public string ip_addr { get; set; }
        public int port { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private SshClient client;
        private string path;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                await Task.Delay(1000, stoppingToken);
            }
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
    }
}
