using Connector;
using Microsoft.AspNetCore.SignalR;

namespace WebAPI.Hubs
{
    public class ConnectorHub : Hub
    {
        public virtual string HubType { get; } = "ReceiveDataUpdate";
        public async Task SendDataUpdate(string nodeName, object value)
        {
            await Clients.All.SendAsync(HubType, nodeName, value);
        }
    }

    public class OpcUaHub : ConnectorHub
    {
        public override string HubType { get; } = "ReceiveDataUpdate";
    }

    public class OpcUaService
    {
        private readonly IHubContext<OpcUaHub> _hubContext;

        public OpcUaService(IHubContext<OpcUaHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyClientsAsync(string nodeName, object value)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveDataUpdate", nodeName, value);
        }
    }

}
