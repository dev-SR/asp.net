using Learn.Entities;
using Microsoft.EntityFrameworkCore;

namespace Learn.Repository.Contracts;

public interface IApplicationDbContext
{
    public DbSet<TestModel> TestModels { get; set; }

}
