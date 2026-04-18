using AI.Nova.Shared.Features.Identity.Dtos;

namespace AI.Nova.Shared.Features.Identity;

[Mapper(UseDeepCloning = true)]
public static partial class IdentityMapper
{
    public static partial void Patch(this UserDto source, UserDto destination);
    public static partial void Patch(this EditUserRequestDto source, UserDto destination);
    public static partial void Patch(this UserDto source, EditUserRequestDto destination);
}
