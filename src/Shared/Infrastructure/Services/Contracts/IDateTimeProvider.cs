namespace AI.Nova.Shared.Infrastructure.Services.Contracts;

public interface IDateTimeProvider
{
    DateTimeOffset GetCurrentDateTime();
}
