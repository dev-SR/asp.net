using Bogus;
using API.Entity.ByConvention;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using API.Entity;

namespace Repository;
public class RepositoryContext : DbContext
{
    public RepositoryContext(DbContextOptions options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }


    public DbSet<Hero> Heros { get; set; }

    public DbSet<Book> Books { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<PriceOffer> PriceOffers { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<BookAuthor> BookAuthors { get; set; }
    public DbSet<BookTag> BookTags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure many-to-many relationships
        modelBuilder.Entity<BookAuthor>().HasKey(ba => new { ba.BookId, ba.AuthorId });
        modelBuilder.Entity<BookAuthor>()
            .HasOne(ba => ba.Book)
            .WithMany(b => b.BookAuthors)
            .HasForeignKey(ba => ba.BookId);
        modelBuilder.Entity<BookAuthor>()
            .HasOne(ba => ba.Author)
            .WithMany(a => a.BookAuthors)
            .HasForeignKey(ba => ba.AuthorId);

        modelBuilder.Entity<BookTag>().HasKey(bt => new { bt.BookId, bt.TagId });
        modelBuilder.Entity<BookTag>()
            .HasOne(bt => bt.Book)
            .WithMany(b => b.BookTags)
            .HasForeignKey(bt => bt.BookId);
        modelBuilder.Entity<BookTag>()
            .HasOne(bt => bt.Tag)
            .WithMany(t => t.BookTags)
            .HasForeignKey(bt => bt.TagId);
        // 1-1
        modelBuilder.Entity<PriceOffer>()
            .HasOne(po => po.Book)
            .WithOne(b => b.PriceOffer)
            .HasForeignKey<PriceOffer>(po => po.BookId);

        Seed(modelBuilder);
    }

    public void Seed(ModelBuilder modelBuilder)
    {
        // Seed Authors
        var authorsFaker = new Faker<Author>()
            .RuleFor(a => a.AuthorId, f => Guid.NewGuid())
            .RuleFor(a => a.Name, f => f.Name.FullName());
        var authors = authorsFaker.Generate(10);

        // Seed Tags
        var tagsFaker = new Faker<Tag>()
            .RuleFor(t => t.TagId, f => Guid.NewGuid())
            .RuleFor(t => t.Name, f => f.Commerce.Categories(1).First());
        var tags = tagsFaker.Generate(5);

        // Seed Books
        var booksFaker = new Faker<Book>()
            .RuleFor(b => b.BookId, f => Guid.NewGuid())
            .RuleFor(b => b.Title, f => f.Lorem.Sentence(3));
        var books = booksFaker.Generate(20);

        // Seed Book-Author relationships
        var bookAuthors = books.SelectMany(book =>
            authors.Take(2).Select(author => new BookAuthor
            {
                BookId = book.BookId,
                AuthorId = author.AuthorId
            })).ToList();

        // Seed Book-Tag relationships
        var bookTags = books.SelectMany(book =>
            tags.Take(2).Select(tag => new BookTag
            {
                BookId = book.BookId,
                TagId = tag.TagId
            })).ToList();

        // Seed Reviews
        var reviewsFaker = new Faker<Review>()
            .RuleFor(r => r.ReviewId, f => Guid.NewGuid())
            .RuleFor(r => r.ReviewText, f => f.Lorem.Paragraph())
            .RuleFor(r => r.BookId, f => f.PickRandom(books).BookId);
        var reviews = reviewsFaker.Generate(50);

        // Seed PriceOffers
        var priceOffers = books
            .Take(15)
            .Select(b => new PriceOffer
            {
                PriceOfferId = Guid.NewGuid(),
                NewPrice = new Faker().Finance.Amount(10, 100),
                BookId = b.BookId,
                Book = null! // Assign a non-null dummy to satisfy the compiler
                /* or  Remove required From the Navigation Property*/
            }).ToList();


        // Add data to the model builder
        modelBuilder.Entity<Author>().HasData(authors);
        modelBuilder.Entity<Tag>().HasData(tags);
        modelBuilder.Entity<Book>().HasData(books);
        modelBuilder.Entity<BookAuthor>().HasData(bookAuthors);
        modelBuilder.Entity<BookTag>().HasData(bookTags);
        modelBuilder.Entity<Review>().HasData(reviews);
        modelBuilder.Entity<PriceOffer>().HasData(priceOffers);
    }
}
