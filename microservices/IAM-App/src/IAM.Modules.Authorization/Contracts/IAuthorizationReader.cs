namespace IAM.Modules.Authorization.Contracts;

public interface IAuthorizationReader
{
    Task<bool> HasRoleAsync(Guid identityId, string roleName, CancellationToken cancellationToken = default);
}
