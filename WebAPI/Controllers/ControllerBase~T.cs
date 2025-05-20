using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;

namespace WebAPI.Controllers;

[ApiController]
[Route("odata/[controller]")]
public abstract class ODataControllerBase<TEntity> : ODataController where TEntity: Entity
{

    protected readonly CustomDbContext Context;

    public ODataControllerBase(CustomDbContext context)
    {
        Context = context;
    }

    // 支持 OData 查询参数，如 $filter、$orderby 等
    protected IEnumerable<TEntity> GetEntities()
    {
        var records = Context.Set<TEntity>().AsQueryable();

        return records; // 注意返回 IQueryable 以启用 OData 查询支持
    }


    // 支持 OData 查询参数，如 $filter、$orderby 等
    protected async Task<IEnumerable<TEntity>> GetEntitiesAsync()
    {
        var records = await Context.Set<TEntity>().ToListAsync();

        return records; // 注意返回 IQueryable 以启用 OData 查询支持
    }
    [ODataIgnored]   //必需  否则OData自带的默认Get方法会冲突
    [HttpGet]
    public virtual IActionResult Get()
    {
        var records = GetEntities();
        return Ok(records);
    }
}
