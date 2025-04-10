using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Background_service
{
    public class SSHConfig
    {
        public string ip_addr { get; set; }
        public int port { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
    public class ProxyConfig
    {
        public string? Proxy { get; set; }
        public string? ProxyBypass { get; set; }
        public bool AutoConfigIsEnabled { get; set; }
        public string? AutoconfigUrl { get; set; }
        public bool AutoDetect { get; set; }
        public bool PerUserProxySettings { get; set; }
    }
    public class Config
    {
        public SSHConfig ssh_config;
        
        public ProxyConfig proxy_config;
        public string proxy_conf;
        public Config() {
            ssh_config = new SSHConfig();
            proxy_config = new ProxyConfig();
            load_config();
        }
        public void load_config()
        {
            string path = $".\\config";
            string file = $"{path}\\ssh.config.json";
            if (File.Exists(file))
            {
                SSHConfig? conf = JsonSerializer.Deserialize<SSHConfig>(File.ReadAllText(file));
                if (conf != null)
                {
                    ssh_config = conf;
                }
            }
            file = $"{path}\\proxy.config.json";
            if (File.Exists(file))
            {
                ProxyConfig? conf = JsonSerializer.Deserialize<ProxyConfig>(File.ReadAllText(file));
                if (conf != null)
                {
                    proxy_config = conf;
                }
            }
        }
        public void save_config() { }
        public string proxy_config_to_json()
        {
            return JsonSerializer.Serialize(proxy_config);
        }

        //void load_config()
        //{
        //    if (File.Exists(path))
        //    {
        //        File.ReadAllText(path);
        //        Config conf = JsonSerializer.Deserialize<Config>(File.ReadAllText(path));
        //        ip_address.Text = conf.ip_addr;
        //        port.Text = String.Concat(conf.port);
        //        username.Text = conf.username;
        //        password.Text = conf.password;
        //    }
        //}
        //void save_config()
        //{
        //    String result = String.Empty;
        //    Config conf = new Config();
        //    conf.ip_addr = ip_address.Text;
        //    conf.port = Convert.ToInt32(port.Text);
        //    conf.username = username.Text;
        //    conf.password = password.Text;

        //    File.WriteAllText(path, JsonSerializer.Serialize<Config>(conf));

        //}
    }
}
