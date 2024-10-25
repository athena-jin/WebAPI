using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebAPI.Data;

namespace WebAPI.Controllers;
[Route("odata/Machines")]
public class MachinesController : ODataController
{
    private readonly CustomDbContext _context;

    public MachinesController(CustomDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_context.Machines);
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

        // Change the status to Running if the machine is currently not running
        if (machine.Status != MachineStatus.Running)
        {
            machine.Status = MachineStatus.Running;
            _context.SaveChanges();
        }

        return Ok(machine);
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

