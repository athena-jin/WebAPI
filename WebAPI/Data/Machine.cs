using Connector;

namespace WebAPI.Data
{
    public partial class Machine
    {
        public ClientBase? _client;
        public ClientBase? Client
        {
            get
            {
                if(_client == null)
                {
                    switch (ConnectorType) 
                    {
                        case MachineConnectorType.OPC_UA:
                            _client = new OpcUaClient(Address,Port);
                            break;
                        default:
                            break;
                    }
                }
                return _client;
            }
        }
    }
}
