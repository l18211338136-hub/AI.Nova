using AI.Nova.Server.Api.Infrastructure.Services.Contracts;

namespace AI.Nova.Server.Api.Infrastructure.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid? GetCurrentUserId()
    {
        var userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (Guid.TryParse(userId, out var guid))
        {
            return guid;
        }

        return null;
    }
}

