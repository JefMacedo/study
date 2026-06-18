namespace IAM.Modules.Authentication.Contracts;

public interface IAuthenticationReader
{
    Task<CredentialDto?> GetCredentialAsync(Guid identityId, CancellationToken cancellationToken = default);
}

public record CredentialDto(Guid IdentityId, string PasswordHash);
