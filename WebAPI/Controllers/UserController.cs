using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using WebAPI.Controllers;
using WebAPI.Data;
using WebAPI.Filters;

/// <summary>
/// 登录控制器
/// </summary>
[ApiController]
[Route("odata/[controller]")]
public class UserController : ODataControllerBase<User>
{
    public UserController(CustomDbContext context/*, IHubContext<User> hubContext*/):base(context)
    {
        //_hubContext = hubContext;
    }
    //已登录的令牌集合
    public static ConcurrentDictionary<Guid, User> Tokens { get; set; } = new ConcurrentDictionary<Guid, User>();

    // 对外提供 Token 验证
    public static bool ValidateToken(Guid token, out User? user)
    {
        return Tokens.TryGetValue(token, out user);
    }

    /// <summary>
    /// 获取所有用户名
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public override IActionResult Get()
    {
        return Ok(GetEntities().Select(_ => _.Name));
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
        var users = GetEntities();

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
        var users = GetEntities();

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
    [HttpPost("Logout")]
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
        var user = Context.Users.FirstOrDefault(_ => _.Name == name);
        var result = Context.Users.Remove(user);
        Context.SaveChanges();
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
        if (GetEntities().Any(_ => _.Name == user.Name))
        {
            return BadRequest($"用户 {user.Name} 已存在");
        }
        var result = Context.Users.Add(user);
        Context.SaveChanges();
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
        var t_user = GetEntities().FirstOrDefault(_ => _.Name == user.Name);
        if (t_user == null)
        {
            Context.Users.Add(user);
            Context.SaveChanges();
            return Ok(user.Name);
        }
        t_user.Password = user.Password;
        var result = Context.Users.Update(t_user);

        Context.SaveChanges();
        return Ok(t_user.Name);
    }
}
