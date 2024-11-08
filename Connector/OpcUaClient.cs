using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using System.Threading.Tasks;
namespace Connector
{
#nullable disable
    

    public class OpcUaClient : ClientBase
    {
        public string DefaultEndpoint => "opc.tcp://localhost:4840";
        private readonly string _endpointUrl;
        private ApplicationInstance _application;
        private Session _session;

        public OpcUaClient(string endpointUrl)
        {
            //$"opc.tcp://{ip}:{port}"  入参例子
            _endpointUrl = endpointUrl;
        }
        public OpcUaClient(string address, uint port) : this($"opc.tcp://{address}:{port}") { }


        public override async Task ConnectAsync()
        {
            _application = new ApplicationInstance
            {
                ApplicationName = "MyOpcUaClient",
                ApplicationType = ApplicationType.Client,
            };

            await _application.CheckApplicationInstanceCertificate(false, 0);

            var endpoint = CoreClientUtils.SelectEndpoint(_endpointUrl, false);
            _session = await Session.Create(
                _application.ApplicationConfiguration,
                new ConfiguredEndpoint(null, endpoint),
                false,
                "MySession",
                60000,
                null,
                null);
        }

        public override async Task DisconnectAsync()
        {
            _application = new ApplicationInstance
            {
                ApplicationName = "MyOpcUaClient",
                ApplicationType = ApplicationType.Client,
            };

            await _application.CheckApplicationInstanceCertificate(false, 0);

            var endpoint = CoreClientUtils.SelectEndpoint(_endpointUrl, false);
            _session = await Session.Create(
                _application.ApplicationConfiguration,
                new ConfiguredEndpoint(null, endpoint),
                false,
                "MySession",
                60000,
                null,
                null);
        }

        public void Subcrition(params string[] nodeNames)
        {
            if(_session == null)
            {
                return;
            }
            // 创建订阅对象
            Subscription subscription = new Subscription(_session.DefaultSubscription)
            {
                PublishingInterval = 1000
            };
            _session.AddSubscription(subscription);
            subscription.Create();

            // 添加监控项 nodeName 示例：  "ns=2;s=MachineStatus"
            // 添加监控项 nodeName 示例：  "ns=2;s=Speed"
            // 添加监控项 nodeName 示例：  "ns=2;s=Temperature"
            foreach (var nodeName in nodeNames)
            {

                MonitoredItem monitoredItem = new MonitoredItem(subscription.DefaultItem)
                {
                    StartNodeId = nodeName,  // 使用你想订阅的节点ID
                    AttributeId = Attributes.Value,
                    SamplingInterval = 1000
                };
                monitoredItem.Notification += OnDataChanged;

                // 将监控项添加到订阅中
                subscription.AddItem(monitoredItem);

            }
            subscription.ApplyChanges();

        }

        private void OnDataChanged(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e)
        {
            foreach (var value in monitoredItem.DequeueValues())
            {
                Console.WriteLine($"节点 {monitoredItem.StartNodeId} 的新值: {value.Value}");
            }
        }

        /// <summary>
        /// 读取指定代码块
        /// </summary>
        /// <param name="nodeName">具体位置</param>
        /// <returns></returns>
        public override async Task<string> ReadDataAsync(string nodeName)
        {
            // 这里是节点 ID，您需要根据实际情况替换
            var nodeId = new NodeId(nodeName);
            var value = await _session.ReadValueAsync(nodeId);
            return value.Value.ToString();
        }

        public override void Dispose()
        {
            _session?.Close();
        }
    }

}
