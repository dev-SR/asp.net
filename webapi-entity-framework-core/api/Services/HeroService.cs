using System;
using API.DTO;
using API.Entity;
using API.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace API.Services;

public class HeroService : IHeroService
{
    private readonly RepositoryContext _db;
    public HeroService(RepositoryContext db)
    {
        _db = db;
    }
    public async Task<List<Hero>> GetAllHeros(bool? isActive)
    {
        if (isActive != null)
        {
            return await _db.Heros.Where(m => m.isActive == isActive).ToListAsync();
        }
        return await _db.Heros.ToListAsync();
    }

    public async Task<Hero?> GetHerosByID(Guid id)
    {
        return await _db.Heros.FirstOrDefaultAsync(hero => hero.Id == id);
    }

    public async Task<Hero?> AddHero(AddUpdateHero obj)
    {
        var addHero = new Hero()
        {
            FirstName = obj.FirstName,
            LastName = obj.LastName,
            isActive = obj.isActive,
        };
        _db.Heros.Add(addHero);
        var result = await _db.SaveChangesAsync();
        return result >= 0 ? addHero : null;
    }

    public async Task<Hero?> UpdateHero(Guid id, AddUpdateHero obj)
    {
        var hero = await _db.Heros.FirstOrDefaultAsync(index => index.Id == id);
        if (hero != null)
        {
            hero.FirstName = obj.FirstName;
            hero.LastName = obj.LastName;
            hero.isActive = obj.isActive;

            var result = await _db.SaveChangesAsync();
            return result >= 0 ? hero : null;
        }
        return null;
    }

    public async Task<bool> DeleteHerosByID(Guid id)
    {
        var hero = await _db.Heros.FirstOrDefaultAsync(index => index.Id == id);
        if (hero != null)
        {
            _db.Heros.Remove(hero);
            var result = await _db.SaveChangesAsync();
            return result >= 0;
        }
        return false;
    }
}

