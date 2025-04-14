using System;
using Learn.Common.Enums;
using Microsoft.AspNetCore.Identity;

namespace Learn.Entities;

public class ApplicationRole(string roleName) : IdentityRole<Guid>(roleName)
{
    public ApplicationRoleStatus Status { get; set; }

    public Guid? CreatedBy { get; set; }
    public DateTime Created { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public IList<ApplicationUserRole> UserRoles { get; set; } = [];
}
