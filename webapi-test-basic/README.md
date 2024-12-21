
## ASP.NET Core Web API UniTesting with XUnit

### Create .NET unit test project with XUnit

1. Inside the root folder, create a new folder for the unit test project named `API.Tests`
2. From the command line, `cd` into the /`root/API.Tests` folder and run the following command to create the unit test project with XUnit:

```bash
dotnet new xunit
```
3. Add this test project to the solution file:

```bash
dotnet sln WebAPISolution.sln add api.test/
```
4. Add reference to the main project - `API` from the unit test project.
5. Delete the auto generated test file `UnitTest1.cs`

### Add some .NET unit tests

Here we install a couple of NuGet packages (`Moq` and `AutoFixture`) that help with setting up test data and mocking dependencies, and add a few initial unit tests for the Service class in the example API project.

1. Install the AutoFixture package from NuGet with the following command: `dotnet add package AutoFixture`
2. Install the Moq package from NuGet with the following command: `dotnet add package Moq`
3. Create a new folder named `Services` in the WebAPI.Tests project
4. Create a new file named `HeroServiceTests.cs` in the `Services` folder with the following tests:

```csharp
using API.DTO;
using API.Model;
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
```


5. Run the tests with the following command:

```bash
dotnet test
```