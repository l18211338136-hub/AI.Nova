namespace AI.Nova.Server.Api.Infrastructure.Services.Contracts;

public interface ICurrentUserService
{
    Guid? GetCurrentUserId();
}
