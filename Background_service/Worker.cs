using Renci.SshNet;
using System.Diagnostics;

namespace Background_service
{
    public partial class Worker : BackgroundServiceRPC.BackgroundService.BackgroundServiceBase
    {
        private SshClient ssh_client;
        private Config config;
        private readonly ILogger<Worker> _logger;
        public Worker() {
            config = new Config();
            ssh_client = new SshClient(config.ssh_config.ip_addr, config.ssh_config.port, config.ssh_config.username, config.ssh_config.password);
        }
        void start_proxy()
        {
            string cmd = "winhttp set advproxy setting-scope=user settings={"+
                $"\\\"Proxy\\\":\\\"{config.proxy_config.Proxy}\\\"," +
                $"\\\"ProxyBypass\\\":\\\"{config.proxy_config.ProxyBypass}\\\"," +
                $"\\\"AutoconfigUrl\\\":\\\"{config.proxy_config.AutoconfigUrl}\\\"," +
                $"\\\"AutoDetect\\\":{config.proxy_config.AutoDetect.ToString().ToLower()}" +
                "}";
            Process process = new Process();
            ProcessStartInfo processStartInfo= new ProcessStartInfo();
            //processStartInfo.WindowStyle = ProcessWindowStyle.Hidden();
            processStartInfo.FileName = "netsh";
            processStartInfo.Arguments = cmd;
            process.StartInfo = processStartInfo;
            process.Start();
        }
        void stop_proxy()
        {
            string cmd = "winhttp set advproxy setting-scope=user settings={" +
                $"\\\"Proxy\\\":\\\"\\\"," +
                $"\\\"ProxyBypass\\\":\\\"\\\"," +
                $"\\\"AutoconfigUrl\\\":\\\"\\\"," +
                $"\\\"AutoDetect\\\":{config.proxy_config.AutoDetect.ToString().ToLower()}" +
                "}";
            Process process = new Process();
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            //processStartInfo.WindowStyle = ProcessWindowStyle.Hidden();
            processStartInfo.FileName = "netsh";
            processStartInfo.Arguments = cmd;
            process.StartInfo = processStartInfo;
            process.Start();
        }

        void connect_ssh()
        {
            ssh_client.Connect();
            if (ssh_client.IsConnected)
            {
                config.save_config();
            }
            var portForward = new ForwardedPortDynamic("127.0.0.1", 6030);
            ssh_client.AddForwardedPort(portForward);
            portForward.Start();

        }
        void disconnect_ssh()
        {
            if (ssh_client != null && ssh_client.IsConnected)
            {
                ssh_client.Disconnect();
            }
        }
    }
}
