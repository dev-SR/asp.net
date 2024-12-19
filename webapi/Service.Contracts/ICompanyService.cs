using System;
using Shared.DTO;

namespace Service.Contracts;

public interface ICompanyService
{
    IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges);
}
