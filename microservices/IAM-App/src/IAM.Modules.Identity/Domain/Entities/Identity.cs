namespace IAM.Modules.Identity.Domain.Entities;

public enum IdentityType { User, Service }
public enum IdentityStatus { Active, Inactive, Suspended }

public class Identity
{
    public Guid Id { get; private set; }
    public Guid? TenantId { get; private set; }
    public string Email { get; private set; } = null!;
    public IdentityType Type { get; private set; }
    public IdentityStatus Status { get; private set; }

    public Identity(Guid id, string email, IdentityType type, IdentityStatus status, Guid? tenantId = null)
    {
        Id = id;
        Email = email;
        Type = type;
        Status = status;
        TenantId = tenantId;
    }
}
