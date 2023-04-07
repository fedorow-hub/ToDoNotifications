using Notes.Application.Interfaces;
using System.Security.Claims;

namespace Notes.WebAPI.Services;

public class CurrentUserService : ICurrentUserService
{
    //IHttpContextAccessor предоставляет доступ к текущему HttpContext, и если он доступен, то от туда
    //мы можем получить информацию о пользователе
    private readonly IHttpContextAccessor _httpContextAccessor;
	public CurrentUserService(IHttpContextAccessor httpContextAccessor)
	{
        _httpContextAccessor = httpContextAccessor;
    }
    public Guid UserId 
    {
        get
        {
            var id = _httpContextAccessor.HttpContext?.User?
                .FindFirstValue(ClaimTypes.NameIdentifier);
            return string.IsNullOrEmpty(id) ? Guid.Empty : Guid.Parse(id);
        }
    }
}
