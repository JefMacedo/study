namespace IAM.Modules.Identity.IntegrationEvents;

public sealed record IdentityCreated(Guid IdentityId, string Email, Guid? TenantId);
