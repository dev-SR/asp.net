using API.DTO;
using API.Entity;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Repository;
using AutoFixture;

namespace api.test.Services
{
    public class HeroServiceTests
    {
        private readonly HeroService _heroService;
        private readonly RepositoryContext _context;
        private readonly Fixture _fixture;

        public HeroServiceTests()
        {
            var options = new DbContextOptionsBuilder<RepositoryContext>()
                .UseInMemoryDatabase(databaseName: "HeroDatabase")
                .Options;
            _context = new RepositoryContext(options);
            _context.Database.EnsureDeleted();
            _heroService = new HeroService(_context);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetAllHeroes_ShouldReturnAllHeroes()
        {
            // Arrange
            var heroes = _fixture.CreateMany<Hero>(3).ToList();
            _context.Heros.AddRange(heroes);
            _context.SaveChanges();

            // Act
            var result = await _heroService.GetAllHeros(null);

            // Assert
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task GetHerosByID_ShouldReturnHero()
        {
            // Arrange
            var hero = _fixture.Create<Hero>();
            _context.Heros.Add(hero);
            _context.SaveChanges();

            // Act
            var result = await _heroService.GetHerosByID(hero.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(hero.Id, result.Id);
        }

        [Fact]
        public async Task AddHero_ShouldAddHero()
        {
            // Arrange
            var heroDto = _fixture.Create<AddUpdateHero>();

            // Act
            var result = await _heroService.AddHero(heroDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(heroDto.FirstName, result.FirstName);
        }

        [Fact]
        public async Task UpdateHero_ShouldUpdateHero()
        {
            // Arrange
            var hero = _fixture.Create<Hero>();
            _context.Heros.Add(hero);
            _context.SaveChanges();
            var heroDto = _fixture.Create<AddUpdateHero>();

            // Act
            var result = await _heroService.UpdateHero(hero.Id, heroDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(heroDto.FirstName, result.FirstName);
        }

        [Fact]
        public async Task DeleteHerosByID_ShouldDeleteHero()
        {
            // Arrange
            var hero = _fixture.Create<Hero>();
            _context.Heros.Add(hero);
            _context.SaveChanges();

            // Act
            var result = await _heroService.DeleteHerosByID(hero.Id);

            // Assert
            Assert.True(result);
        }
    }
}