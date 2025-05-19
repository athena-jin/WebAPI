using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using WebAPI.Data;
using WebAPI.Filters;
using WebAPI.Hubs;

/// <summary>
/// 登录控制器
/// </summary>
[ApiController]
[Route("odata/[controller]")]
public class UserController : ODataController
{
    private readonly CustomDbContext _context;
    //private readonly IHubContext<User> _hubContext;

    public UserController(CustomDbContext context/*, IHubContext<User> hubContext*/)
    {
        _context = context;
        //_hubContext = hubContext;
    }
    //已登录的令牌集合
    public static ConcurrentDictionary<Guid, User> Tokens { get; set; } = new ConcurrentDictionary<Guid, User>();

    // 对外提供 Token 验证
    public static bool ValidateToken(Guid token, out User? user)
    {
        return Tokens.TryGetValue(token, out user);
    }

    private IEnumerable<User> Get()
    {
        return _context.Users;
    }


    /// <summary>
    /// 获取所有用户名
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [TokenAuth]
    public IActionResult Query()
    {
        return Ok(Get().Select(_ => _.Name));
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="user">登录User表单</param>
    /// <returns></returns>
    [HttpPost("Login")]
    public IActionResult Login([FromBody] User user)
    {
        //todo 返回登录令牌
        if (Tokens.Any(_ => _.Value.Name == user.Name))
        {
            return BadRequest($"用户 {user.Name} 已登录，请先退出");
        }
        var users = Get();

        if (users.FirstOrDefault(_ => _.Name == user.Name) is User t_user)
        {
            if (t_user.Password == user.Password)
            {
                Guid token = Guid.NewGuid();
                Tokens.TryAdd(token, t_user);
                return Ok(token);
            }
            else
            {
                return BadRequest("密码错误");
            }
        }
        return BadRequest($"用户 {user.Name} 不存在");

    }


    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="user">登录User表单</param>
    /// <returns></returns>
    [HttpPost("Login/{name}/{password}")]
    public IActionResult LoginByRounte(string name,string password)
    {
        //todo 返回登录令牌
        if (Tokens.Any(_ => _.Value.Name == name))
        {
            return BadRequest($"用户 {name} 已登录，请先退出");
        }
        var users = Get();

        if (users.FirstOrDefault(_ => _.Name == name) is User t_user)
        {
            if (t_user.Password == password)
            {
                Guid token = Guid.NewGuid();
                Tokens.TryAdd(token, t_user);
                return Ok(token);
            }
            else
            {
                return BadRequest("密码错误");
            }
        }
        return BadRequest($"用户 {name} 不存在");

    }

    /// <summary>
    /// 注销
    /// </summary>
    /// <returns></returns>
    [HttpPost("Logout/{name}")]
    [TokenAuth]
    public IActionResult Logout([FromHeader(Name = "Authorization")] Guid token)
    {
        if (Tokens.TryRemove(token, out var user))
        {
            return Ok($"{user.Name} 已退出");
        }
        return BadRequest("令牌无效或用户未登录");
    }

    /// <summary>
    /// 删除指定实体
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    [HttpPost("Delete/{name}")]
    [TokenAuth]
    public IActionResult Delete(string name)
    {
        //此处需要判断是否在登录中
        var user = _context.Users.FirstOrDefault(_ => _.Name == name);
        var result = _context.Users.Remove(user);
        _context.SaveChanges();
        return Ok();
    }
    /// <summary>
    /// 新增实体
    /// </summary>
    /// <param name="user">新增User表单</param>
    /// <returns></returns>
    [HttpPost("Add")]
    [TokenAuth]
    public IActionResult Add([FromBody] User user)
    {
        //检查是否已存在
        if (Get().Any(_ => _.Name == user.Name))
        {
            return BadRequest($"用户 {user.Name} 已存在");
        }
        var result = _context.Users.Add(user);
        _context.SaveChanges();
        return Ok(user.Name);
    }
    /// <summary>
    /// 更新实体
    /// </summary>
    /// <param name="user">编辑User表单</param>
    /// <returns></returns>
    [HttpPost("Update")]
    [TokenAuth]
    public IActionResult Update([FromBody] User user)
    {
        var t_user = Get().FirstOrDefault(_ => _.Name == user.Name);
        if (t_user == null)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok(user.Name);
        }
        t_user.Password = user.Password;
        var result = _context.Users.Update(t_user);

        _context.SaveChanges();
        return Ok(t_user.Name);
    }
}
