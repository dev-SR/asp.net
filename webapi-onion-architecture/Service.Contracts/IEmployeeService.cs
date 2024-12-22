using System;
using Shared.DTO;

namespace Service.Contracts;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetEmployees(Guid companyId, bool trackChanges);
    Task<EmployeeDto?> GetEmployee(Guid companyId, Guid id, bool trackChanges);
    Task<EmployeeDto?> CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges);

    Task DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges);

    Task UpdateEmployeeForCompany(Guid companyId, Guid id,
                                    EmployeeForUpdateDto employeeForUpdate,
                                    bool compTrackChanges,
                                    bool empTrackChanges);
}
