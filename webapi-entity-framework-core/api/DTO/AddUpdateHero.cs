using System;

namespace API.DTO;

public class AddUpdateHero
{
    public required string FirstName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public bool isActive { get; set; } = true;
}