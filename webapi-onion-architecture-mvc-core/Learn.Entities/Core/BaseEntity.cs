namespace Learn.Entities.Core;

public class BaseEntity
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; } = false;
    public bool IsActive { get; set; } = true;
}


