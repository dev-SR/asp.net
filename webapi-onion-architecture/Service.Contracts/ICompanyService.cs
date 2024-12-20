using Shared.DTO;

namespace Service.Contracts;

public interface ICompanyService
{
    IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges);
    CompanyDto? GetCompany(Guid companyId, bool trackChanges);
    CompanyDto CreateCompany(CompanyForCreationDto company);
    void DeleteCompany(Guid companyId, bool trackChanges);
    void UpdateCompany(Guid companyId, CompanyForUpdateDto companyForUpdate, bool trackChanges);
}
