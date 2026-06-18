namespace IAM.Modules.Authorization.Domain.Entities;

public class Permission
{
    public Guid Id { get; private set; }
    public string Code { get; private set; } = null!;

    public Permission(Guid id, string code)
    {
        Id = id;
        Code = code;
    }
}
