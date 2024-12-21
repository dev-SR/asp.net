using System;
using API.DTO;
using API.Model;

namespace API.Services.Contracts;

public interface IHeroService
{
    Task<List<Hero>> GetAllHeros(bool? isActive);
    Task<Hero?> GetHerosByID(Guid id);
    Task<Hero?> AddHero(AddUpdateHero obj);
    Task<Hero?> UpdateHero(Guid id, AddUpdateHero obj);
    Task<bool> DeleteHerosByID(Guid id);
}
