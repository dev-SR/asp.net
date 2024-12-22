namespace API.Entity.ByConvention;

public class Book
{
    public Guid BookId { get; set; }
    public required string Title { get; set; }
    public virtual ICollection<Review>? Reviews { get; set; } // 1-M Navigation property
    public virtual PriceOffer? PriceOffer { get; set; } // 1-0/1 Navigation property
    public virtual ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>(); // Join table for Book-Author
    public virtual ICollection<BookTag> BookTags { get; set; } = new List<BookTag>(); // Join table for Book-Tag
}

public class Review
{
    public Guid ReviewId { get; set; }
    public required string ReviewText { get; set; }
    public Guid BookId { get; set; } // Foreign key

    public required Book Book { get; set; }  // Navigation property
}

public class PriceOffer
{
    public Guid PriceOfferId { get; set; }
    public decimal NewPrice { get; set; }
    public Guid BookId { get; set; } // Foreign key
    public required Book Book { get; set; }  // Navigation property
}

public class Author
{
    public Guid AuthorId { get; set; }
    public required string Name { get; set; }
    public virtual ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>(); // Join table for Book-Author
}

public class Tag
{
    public Guid TagId { get; set; }
    public required string Name { get; set; }
    public virtual ICollection<BookTag> BookTags { get; set; } = new List<BookTag>(); // Join table for Book-Tag
}

public class BookAuthor
{
    public Guid BookId { get; set; }
    public Book Book { get; set; } = null!;
    public Guid AuthorId { get; set; }
    public Author Author { get; set; } = null!;
}

public class BookTag
{
    public Guid BookId { get; set; }
    public Book Book { get; set; } = null!;
    public Guid TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}
