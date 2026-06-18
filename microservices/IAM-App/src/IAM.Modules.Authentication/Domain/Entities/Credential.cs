namespace IAM.Modules.Authentication.Domain.Entities;

public class Credential
{
    public Guid IdentityId { get; private set; }
    public string PasswordHash { get; private set; } = null!;

    public Credential(Guid identityId, string passwordHash)
    {
        IdentityId = identityId;
        PasswordHash = passwordHash;
    }
}
