using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;

namespace WebAPI.Controllers;

[ApiController]
[Route("odata/[controller]")]
public class WarningRecordController : ODataControllerBase<WarningRecord>
{

    public WarningRecordController(CustomDbContext context):base(context)
    {

    }


    //[HttpGet]
    //public override IActionResult Get()
    //{
    //    var records = GetEntities();
    //    return Ok(records);
    //}
}
