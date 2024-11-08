using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebAPI.Data;
using Connector;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using WebAPI.Hubs;

namespace WebAPI.Controllers;
[Route("odata/Machines")]
public class MachinesController : ODataController
{
    private readonly CustomDbContext _context;
    private readonly IHubContext<OpcUaHub> _hubContext;

    public MachinesController(CustomDbContext context, IHubContext<OpcUaHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_context.Machines);
    }
    [HttpPost("{key}/Connect")]
    public IActionResult Connect([FromRoute] Guid key)
    {
        var machine = _context.Machines.Find(key);
        if (machine == null)
        {
            return NotFound();
        }
        else if(machine.Client == null)
        {
            return BadRequest($"Client is null,Address:{machine.Address};Port:{machine.Port};Connect Type:{machine.ConnectorType}");
        }
        machine.Client.ConnectAsync();
        machine.Status = MachineStatus.Running;
        return Ok(machine.Status);
    }

    [HttpGet("{key}/Details")]
    public IActionResult Details([FromRoute] Guid key)
    {
        var machine = _context.Machines.Find(key);
        if (machine?.Details == null)
        {
            return NotFound();
        }
        return Ok(machine.Details);
    }

    private async Task<string> GetMachineDetailsAsync(Machine machine)
    {
        Dictionary<string, string> details = new Dictionary<string, string>() { };
        //switch (machine.ConnectorType)
        //{
        //    case MachineConnectorType.OPC_UA:
        //        using (var opcClient = new OpcUaClient(machine.Address, machine.Port))
        //        {
        //            await opcClient.ConnectAsync();
        //            var temperature = await opcClient.ReadNodeDataAsync("ns=2;i=2");
        //            var speed = await opcClient.ReadNodeDataAsync("ns=2;i=2");
        //        }
        //        break;
        //}
        // TODO: 实现 OPC UA 或 Sharp7 客户端以获取机器详细信息
        // 这里是一个示例，您需要根据您的实际实现替换

        // 例如：使用 OPC UA 客户端
        /*
        using (var opcClient = new OpcUaClient(machine.Address, machine.Port))
        {
            await opcClient.ConnectAsync();
            var temperature = await opcClient.ReadTemperatureAsync();
            var speed = await opcClient.ReadSpeedAsync();

            return new MachineDetails
            {
                // 根据读取到的数据填充详细信息
            };
        }
        */

        // 或者使用 Sharp7 协议
        /*
        using (var sharp7Client = new Sharp7Client(machine.Address, machine.Port))
        {
            await sharp7Client.ConnectAsync();
            var data = await sharp7Client.ReadAsync();
            // 处理读取到的数据

            return new MachineDetails
            {
                // 根据读取到的数据填充详细信息
            };
        }
        */

        string json = JsonSerializer.Serialize(details);
        return json;
    }
    [HttpGet("{key}")]
    public IActionResult Get([FromRoute] Guid key)
    {
        var machine = _context.Machines.Find(key);
        if (machine == null)
        {
            return NotFound();
        }
        return Ok(machine);
    }

    [HttpPost("{key}/Open")]
    public IActionResult Open([FromRoute]Guid key)
    {
        var machine = _context.Machines.Find(key);
        if (machine == null)
        {
            return NotFound();
        }
        else if (machine.Client == null)
        {
            return BadRequest($"Client is null,Address:{machine.Address};Port:{machine.Port};Connect Type:{machine.ConnectorType}");
        }
        machine.Client.ConnectAsync();
        machine.Status = MachineStatus.Running;
        _context.SaveChanges();
        return Ok(machine.Status);
    }

    [HttpPost("{key}/Close")]
    public IActionResult Close([FromRoute] Guid key)
    {
        var machine = _context.Machines.Find(key);
        if (machine == null)
        {
            return NotFound();
        }

        // Change the status to Closed if the machine is currently running
        if (machine.Status == MachineStatus.Running)
        {
            machine.Status = MachineStatus.Closed;
            _context.SaveChanges();
        }

        return Ok(machine);
    }
}

