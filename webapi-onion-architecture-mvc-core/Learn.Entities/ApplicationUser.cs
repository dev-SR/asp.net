using System.ComponentModel.DataAnnotations;
using Learn.Common.Enums;
using Microsoft.AspNetCore.Identity;
namespace Learn.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    [Display(Name = "Full Name")]
    public required string FullName { get; set; }
    public string? ImageUrl { get; set; }
    public string? LastPassword { get; set; }
    public DateTime? LastPassChangeDate { get; set; }
    public int PasswordChangedCount { get; set; }
    public ApplicationUserStatus Status { get; set; }

    public Guid? CreatedBy { get; set; }
    public DateTime Created { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public IList<ApplicationUserRole> UserRoles { get; set; } = [];
}