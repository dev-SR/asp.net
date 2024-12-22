namespace API.Entity;

public class Hero
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public bool isActive { get; set; } = true;
}