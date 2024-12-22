using Shared.DTO;

namespace Service.Contracts;

public interface ICompanyService
{
    Task<IEnumerable<CompanyDto>> GetAllCompanies(bool trackChanges);
    Task<CompanyDto?> GetCompany(Guid companyId, bool trackChanges);
    Task<CompanyDto> CreateCompany(CompanyForCreationDto company);
    Task DeleteCompany(Guid companyId, bool trackChanges);
    Task UpdateCompany(Guid companyId, CompanyForUpdateDto companyForUpdate, bool trackChanges);
}
