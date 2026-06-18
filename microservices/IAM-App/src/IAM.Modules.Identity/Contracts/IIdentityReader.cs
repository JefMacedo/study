namespace IAM.Modules.Identity.Contracts;

public interface IIdentityReader
{
    Task<IamIdentityDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}

public record IamIdentityDto(Guid Id, string Email, string? TenantId, string Type, string Status);
