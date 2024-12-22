using System;
using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Logger.Contract;
using Service.Contracts;
using Shared.DTO;
namespace Service;

internal sealed class CompanyService : ICompanyService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;

    private readonly IMapper _mapper;
    public CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _mapper = mapper;
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<CompanyDto>> GetAllCompanies(bool trackChanges)
    {
        var companies = await _repository.Company.GetAllCompanies(trackChanges);
        // var companiesDto = companies.Select(c => new CompanyDto(c.Id, c.Name ?? "",
        //                                     string.Join("", c.Address, c.Country))).ToList();
        var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
        return companiesDto;

    }

    public async Task<CompanyDto?> GetCompany(Guid id, bool trackChanges)
    {
        var company = await _repository.Company.GetCompany(id, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(id);


        var companyDto = _mapper.Map<CompanyDto>(company);
        return companyDto;
    }

    public async Task<CompanyDto?> CreateCompany(CompanyForCreationDto company)
    {
        var companyEntity = _mapper.Map<Company>(company);

        _repository.Company.CreateCompany(companyEntity);
        await _repository.SaveAsync();
        var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);
        return companyToReturn;
    }

    public async Task DeleteCompany(Guid companyId, bool trackChanges)
    {
        var company = await _repository.Company.GetCompany(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);
        _repository.Company.DeleteCompany(company);
        await _repository.SaveAsync();
    }

    public async Task UpdateCompany(Guid companyId, CompanyForUpdateDto companyForUpdate, bool trackChanges)
    {
        var companyEntity = await _repository.Company.GetCompany(companyId, trackChanges);
        if (companyEntity is null)
            throw new CompanyNotFoundException(companyId);

        _mapper.Map(companyForUpdate, companyEntity);
        await _repository.SaveAsync();
    }
}