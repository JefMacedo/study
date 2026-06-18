namespace IAM.Modules.Authorization.Domain.Entities;

public class Role
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;

    public Role(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}
