using AI.Nova.Client.Core.Infrastructure.Services.Contracts;

namespace AI.Nova.Tests.Services;

public partial class TestAuthTokenProvider : IAuthTokenProvider
{
    [AutoInject] private IStorageService storageService = default!;

    public async Task<string?> GetAccessToken()
    {
        return await storageService.GetItem("access_token");
    }
}
