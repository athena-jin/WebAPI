using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPI.Controllers;

namespace WebAPI.Filters;

/// <summary>
/// 令牌验证特性
/// </summary>
public class TokenAuthAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        //此处需要前端支持
        var strToken = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(strToken) || !Guid.TryParse(strToken, out Guid token) || !UserController.ValidateToken(token, out _))
        {
            context.Result = new UnauthorizedObjectResult("未授权或令牌无效");
        }
    }
}
