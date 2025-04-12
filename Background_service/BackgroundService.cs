using Grpc.Core;
using BackgroundServiceRPC;

namespace Background_service
{
    public partial class Worker : BackgroundServiceRPC.BackgroundService.BackgroundServiceBase
    {
        public override Task<Configuration> GetConfig(Empty request, ServerCallContext context)
        {
            return Task.FromResult(new Configuration
            {
                IpAddress = config.ssh_config.ip_addr,
                Port = config.ssh_config.port.ToString(),    
                Username = config.ssh_config.username,
                Password = config.ssh_config.password,
            });
        }

        public override Task<Empty> SetConfig(Configuration request, ServerCallContext context)
        {
            config.ssh_config.ip_addr = request.IpAddress;
            config.ssh_config.port = Convert.ToInt32(request.Port);
            config.ssh_config.username = request.Username;
            config.ssh_config.password = request.Password;
            config.save_config();
            return Task.FromResult(new Empty { });
        }

        public override Task<Empty> ConnectSSH(Empty request, ServerCallContext context)
        {
            connect_ssh();
            start_proxy();
            return Task.FromResult(new Empty { });
        }

        public override Task<Empty> DisconnectSSH(Empty request, ServerCallContext context)
        {
            disconnect_ssh();
            stop_proxy();
            return Task.FromResult(new Empty { });
        }
    }
}
