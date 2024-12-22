namespace API.Controllers;

using API.Entity.ByConvention;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository;

[Route("api/Books")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly RepositoryContext _context;

    public BooksController(RepositoryContext context)
    {
        _context = context;
    }

    // GET: api/Books
    [HttpGet]
    public async Task<IActionResult> GetBooks()
    {
        var books = await _context.Books
            .Select(b => new
            {
                b.BookId,
                b.Title,
                Reviews = b.Reviews.Select(r => new
                {
                    r.ReviewId,
                    r.ReviewText
                }),
                PriceOffer = b.PriceOffer != null ? new
                {
                    b.PriceOffer.PriceOfferId,
                    b.PriceOffer.NewPrice
                } : null,
                Authors = b.BookAuthors.Select(ba => ba.Author.Name),
                Tags = b.BookTags.Select(bt => bt.Tag.Name)
            })
            .ToListAsync();

        return Ok(books);
    }

    // GET: api/Books/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBook(Guid id)
    {
        var book = await _context.Books
            .Include(b => b.Reviews)
            .Include(b => b.PriceOffer)
            .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
            .Include(b => b.BookTags)
                .ThenInclude(bt => bt.Tag)
            .Select(b => new
            {
                b.BookId,
                b.Title,
                Reviews = b.Reviews.Select(r => new
                {
                    r.ReviewId,
                    r.ReviewText
                }),
                PriceOffer = b.PriceOffer != null ? new
                {
                    b.PriceOffer.PriceOfferId,
                    b.PriceOffer.NewPrice
                } : null,
                Authors = b.BookAuthors.Select(ba => ba.Author.Name),
                Tags = b.BookTags.Select(bt => bt.Tag.Name)
            })
            .FirstOrDefaultAsync(b => b.BookId == id);

        if (book == null) return NotFound();

        return Ok(book);
    }

    // DELETE: api/Books/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(Guid id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) return NotFound();

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
