using Bogus;
using Entities.Models;

namespace Repository.Seeding
{
    public static class SeedData
    {
        public static List<Company> Companies { get; private set; } = new List<Company>();
        public static List<Employee> Employees { get; private set; } = new List<Employee>();

        public static void SeedCompanies(int companyCount)
        {
            var companyFaker = new Faker<Company>()
                .RuleFor(c => c.Id, f => Guid.NewGuid())
                .RuleFor(c => c.Name, f => f.Company.CompanyName())
                .RuleFor(c => c.Address, f => f.Address.FullAddress())
                .RuleFor(c => c.Country, f => f.Address.Country());

            Companies = companyFaker.Generate(companyCount);
        }

        public static void SeedEmployees(int employeeCount)
        {
            if (Companies.Count == 0)
                throw new InvalidOperationException("Companies must be seeded before employees.");

            var employeeFaker = new Faker<Employee>()
                .RuleFor(e => e.Id, f => Guid.NewGuid())
                .RuleFor(e => e.Name, f => f.Name.FullName())
                .RuleFor(e => e.Age, f => f.Random.Int(20, 60))
                .RuleFor(e => e.Position, f => f.Name.JobTitle())
                .RuleFor(e => e.CompanyId, f => f.PickRandom(Companies).Id);

            Employees = employeeFaker.Generate(employeeCount);
        }
    }
}
