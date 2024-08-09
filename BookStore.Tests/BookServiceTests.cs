using BookStore.Model;
using BookStore.Services;
using Moq;
using Xunit;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System;
namespace BookStore.Tests
{
    public class BookServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // 每个测试使用唯一的数据库
                .Options;

            _context = new ApplicationDbContext(options);
            var logger = new LoggerFactory().CreateLogger<BookService>();
            _bookService = new BookService(logger, _context);
        }

        [Fact]
        public async Task CreateBookAsync_ShouldCreateBook()
        {
            var book = new Book
            {
                Title = "testbook",
                Author = "testauthor",
                ISBN = "1234567890"
            };

            var result = await _bookService.CreateBookAsync(book);

            Assert.Equal("testbook", result.Title);
            Assert.NotEqual(0, result.Id);
        }

        [Fact]
        public async Task GetBookByIdAsync_ShouldReturnBook_WhenBookExists()
        {
            var book = new Book
            {
                Title = "testbook",
                Author = "testauthor",
                ISBN = "1234567890"
            };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            var result = await _bookService.GetBookByIdAsync(book.Id);

            Assert.NotNull(result);
            Assert.Equal("testbook", result.Title);
        }

        [Fact]
        public async Task GetBookByIdAsync_ShouldReturnNull_WhenBookDoesNotExist()
        {
            var result = await _bookService.GetBookByIdAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllBooksAsync_ShouldReturnAllBooks()
        {
            await _context.Books.AddRangeAsync(
                new Book { Title = "book1", Author = "author1", ISBN = "1111111111" },
                new Book { Title = "book2", Author = "author2", ISBN = "2222222222" }
            );
            await _context.SaveChangesAsync();

            var result = await _bookService.GetAllBooksAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task UpdateBookAsync_ShouldUpdateBook()
        {
            var book = new Book
            {
                Title = "testbook",
                Author = "testauthor",
                ISBN = "1234567890"
            };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            book.Title = "updatedbook";
            var result = await _bookService.UpdateBookAsync(book);

            Assert.True(result);
            var updatedBook = await _context.Books.FindAsync(book.Id);
            Assert.Equal("updatedbook", updatedBook.Title);
        }

        [Fact]
        public async Task DeleteBookAsync_ShouldDeleteBook()
        {
            var book = new Book
            {
                Title = "testbook",
                Author = "testauthor",
                ISBN = "1234567890"
            };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            var result = await _bookService.DeleteBookAsync(book.Id);

            Assert.True(result);
            var deletedBook = await _context.Books.FindAsync(book.Id);
            Assert.Null(deletedBook);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}