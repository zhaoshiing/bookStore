using BookStore.Model;
using Microsoft.EntityFrameworkCore;
using BookStore.Services;

namespace BookStore.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BookService> _logger;

        public BookService(ILogger<BookService> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<Book> CreateBookAsync(Book book)
        {
            _logger.LogInformation("Creating book with title {Title}", book.Title);

            try
            {
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Book created with ID {BookId}", book.Id);
                return book;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating book with title {Title}", book.Title);
                throw;
            }
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            _logger.LogInformation("Fetching book with ID {BookId}", id);

            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    _logger.LogWarning("Book with ID {BookId} not found", id);
                    return null;
                }

                _logger.LogInformation("Book with ID {BookId} fetched successfully", id);
                return book;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching book with ID {BookId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            _logger.LogInformation("Fetching all books");

            try
            {
                var books = await _context.Books.ToListAsync();
                _logger.LogInformation("{Count} books fetched successfully", books.Count);
                return books;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all books");
                throw;
            }
        }

        public async Task<bool> UpdateBookAsync(Book book)
        {
            _logger.LogInformation("Updating book with ID {BookId}", book.Id);

            try
            {
                _context.Books.Update(book);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Book with ID {BookId} updated successfully", book.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating book with ID {BookId}", book.Id);
                throw;
            }
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            _logger.LogInformation("Deleting book with ID {BookId}", id);

            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    _logger.LogWarning("Book with ID {BookId} not found", id);
                    return false;
                }

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Book with ID {BookId} deleted successfully", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting book with ID {BookId}", id);
                throw;
            }
        }
    }
}

