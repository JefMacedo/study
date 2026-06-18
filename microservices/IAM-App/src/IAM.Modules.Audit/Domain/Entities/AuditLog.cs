namespace IAM.Modules.Audit.Domain.Entities;

public class AuditLog
{
    public Guid Id { get; private set; }
    public Guid? IdentityId { get; private set; }
    public string Action { get; private set; } = null!;
    public DateTime OccurredAt { get; private set; }

    public AuditLog(Guid id, string action, Guid? identityId = null)
    {
        Id = id;
        Action = action;
        IdentityId = identityId;
        OccurredAt = DateTime.UtcNow;
    }
}
