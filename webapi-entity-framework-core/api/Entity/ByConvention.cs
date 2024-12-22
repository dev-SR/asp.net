namespace API.Entity.ByConvention;

public class Book
{
    public int BookId { get; set; }
    public required string Title { get; set; }
    public virtual ICollection<Review>? Reviews { get; set; } // 1-M Navigation property
    public virtual PriceOffer? PriceOffer { get; set; } // 1-0/1 Navigation property
    public virtual required ICollection<Author> Authors { get; set; } = new List<Author>(); // Navigation property
    public ICollection<Tag>? Tags { get; set; } = new List<Tag>(); // Navigation property
}

public class Review
{
    public int ReviewId { get; set; }
    public required string ReviewText { get; set; }
    public int BookId { get; set; } // Foreign key
    public required Book Book { get; set; }  // Navigation property
}

public class PriceOffer
{
    public int PriceOfferId { get; set; }
    public decimal NewPrice { get; set; }
    public int BookId { get; set; } // Foreign key
    public required Book Book { get; set; }  // Navigation property
}

public class Author
{
    public int AuthorId { get; set; }
    public required string Name { get; set; }
    public ICollection<Book>? Books { get; set; } = new List<Book>(); // Navigation property
}

public class Tag
{
    public int TagId { get; set; }
    public required string Name { get; set; }
    public ICollection<Book>? Books { get; set; } = new List<Book>(); // Navigation property
}
