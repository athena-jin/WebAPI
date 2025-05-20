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

    // ֧�� OData ��ѯ�������� $filter��$orderby ��
    protected IEnumerable<TEntity> GetEntities()
    {
        var records = Context.Set<TEntity>().AsQueryable();

        return records; // ע�ⷵ�� IQueryable ������ OData ��ѯ֧��
    }


    // ֧�� OData ��ѯ�������� $filter��$orderby ��
    protected async Task<IEnumerable<TEntity>> GetEntitiesAsync()
    {
        var records = await Context.Set<TEntity>().ToListAsync();

        return records; // ע�ⷵ�� IQueryable ������ OData ��ѯ֧��
    }
    [ODataIgnored]   //����  ����OData�Դ���Ĭ��Get�������ͻ
    [HttpGet]
    public virtual IActionResult Get()
    {
        var records = GetEntities();
        return Ok(records);
    }
}
