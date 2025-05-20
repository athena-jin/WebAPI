namespace Connector
{
    public interface IClient:IDisposable
    {
        string Endpoint { get; set; }
        string Address { get; set; }
        uint Port { get; set; }
        Task ConnectAsync();
        Task DisconnectAsync();
        Task<string> ReadDataAsync(string NodeName);
    }
    public abstract class ClientBase : IClient
    {
        public string Endpoint { get; set; }
        public string Address { get; set; }
        public uint Port { get; set; }

        public abstract Task ConnectAsync();
        
        public abstract Task DisconnectAsync();

        public abstract Task<string> ReadDataAsync(string NodeName);

        public abstract void Dispose();
    }
}
