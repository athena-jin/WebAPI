using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;
using Microsoft.AspNetCore.SignalR;
using WebAPI.Hubs;

namespace WebAPI.Controllers;
[Route("odata/[controller]")]
public class MachinesController : ODataControllerBase<Machine>
{
    private readonly IHubContext<OpcUaHub> _hubContext;

    public MachinesController(CustomDbContext context, IHubContext<OpcUaHub> hubContext):base(context)
    {
        _hubContext = hubContext;
    }
    [HttpPost("{key}/Connect")]
    public async Task<IActionResult> Connect([FromRoute] Guid key)
    {
        var machine = Context.Machines.Find(key);
        if (machine == null)
        {
            return NotFound();
        }
        else if(machine.Client == null)
        {
            return BadRequest($"Client is null,Address:{machine.Address};Port:{machine.Port};Connect Type:{machine.ConnectorType}");
        }
        await machine.Client.ConnectAsync();
        machine.Status = MachineStatus.Running;
        return Ok(machine.Status);
    }

    [HttpGet("{key}/Details")]
    public IActionResult Details([FromRoute] Guid key)
    {
        var machine = Context.Machines.Find(key);
        if (machine?.Details == null)
        {
            return NotFound();
        }
        return Ok(machine.Details);
    }

    [HttpGet("{key}")]
    public IActionResult Get([FromRoute] Guid key)
    {
        var machine = Context.Machines.Find(key);
        if (machine == null)
        {
            return NotFound();
        }
        return Ok(machine);
    }

    [HttpPost("{key}/Open")]
    public async Task<IActionResult> Open([FromRoute]Guid key)
    {
        var machine = Context.Machines.Find(key);
        if (machine == null)
        {
            return NotFound();
        }
        else if (machine.Client == null)
        {
            return BadRequest($"Client is null,Address:{machine.Address};Port:{machine.Port};Connect Type:{machine.ConnectorType}");
        }
        await machine.Client.ConnectAsync();
        machine.Status = MachineStatus.Running;
        Context.SaveChanges();
        return Ok(machine.Status);
    }

    [HttpPost("{key}/Close")]
    public IActionResult Close([FromRoute] Guid key)
    {
        var machine = Context.Machines.Find(key);
        if (machine == null)
        {
            return NotFound();
        }

        // Change the status to Closed if the machine is currently running
        if (machine.Status == MachineStatus.Running)
        {
            machine.Status = MachineStatus.Closed;
            Context.SaveChanges();
        }

        return Ok(machine);
    }
}

