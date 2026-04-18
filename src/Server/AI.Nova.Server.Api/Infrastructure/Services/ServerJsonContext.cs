using Fido2NetLib;
using AI.Nova.Shared.Features.Statistics;
using AI.Nova.Server.Api.Features.Identity.Services;

namespace AI.Nova.Server.Api.Infrastructure.Services;

/// <summary>
/// https://devblogs.microsoft.com/dotnet/try-the-new-system-text-json-source-generator/
/// </summary>
[JsonSourceGenerationOptions(
  AllowTrailingCommas = true,
  PropertyNameCaseInsensitive = true,
  GenerationMode = JsonSourceGenerationMode.Default,
  DictionaryKeyPolicy = JsonKnownNamingPolicy.CamelCase,
  PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase
)]
[JsonSerializable(typeof(NugetStatsDto))]
[JsonSerializable(typeof(GoogleRecaptchaVerificationResponse))]
[JsonSerializable(typeof(AuthenticatorResponse))]
public partial class ServerJsonContext : JsonSerializerContext
{
}
