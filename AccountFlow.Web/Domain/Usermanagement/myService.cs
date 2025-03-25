using System;

namespace AccountFlow.Web.Domain.Usermanagement;

public class MyService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MyService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsLoggedIn()
    {
        return _httpContextAccessor.HttpContext.Session.GetString("LoggedIn") == "true";
    }
}
