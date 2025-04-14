using Microsoft.EntityFrameworkCore.Storage;

namespace Learn.Repository.Contracts;

public interface IUnitOfWork
{
    Task<bool> CompleteAsync();
    Task<IDbContextTransaction> BeginTransaction();
}