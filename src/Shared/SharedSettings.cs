using Microsoft.Extensions.Caching.Memory;

namespace AI.Nova.Shared;

public partial class SharedSettings : IValidatableObject
{
    public ApplicationInsightsOptions? ApplicationInsights { get; set; }

    public MemoryCacheOptions MemoryCache { get; set; } = default!;

    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validationResults = new List<ValidationResult>();

        if (ApplicationInsights is not null)
        {
            Validator.TryValidateObject(ApplicationInsights, new ValidationContext(ApplicationInsights), validationResults, true);
        }

        return validationResults;
    }
}

public class ApplicationInsightsOptions
{
    public string? ConnectionString { get; set; }
}
