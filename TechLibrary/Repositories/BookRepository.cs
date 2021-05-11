using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechLibrary.Data;
using TechLibrary.Domain;

namespace TechLibrary.Repositories
{
    public interface IBookRepository
    {
        Task<List<Book>> GetBooksAsync();
        Task<List<Book>> GetBooksAsync(int pageNumber, int pageSize);
        Task<Book> GetBookByIdAsync(int bookid);
        Task<List<Book>> GetBooksByTitle(string title);
        Task<List<Book>> GetBooksByDescription(string description);
        Task<IList<Book>> GetBooksByTitleAndDescription(string title, string description);
        void UpdateBookAsync(Book book);
        void AddBookAsync(Book book);
    }
    public class BookRepository : BaseRepository, IBookRepository
    {
        public BookRepository(DataContext context) : base(context)
        {
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            var queryable = _context.Books.AsQueryable();

            return await queryable.ToListAsync();
        }

        public async Task<List<Book>> GetBooksAsync(int pageNumber, int pageSize)
        {
            var queryable = _context.Books.AsQueryable();
            return await queryable.OrderBy(b => b.BookId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(int bookid)
        {
            // return await _dataContext.Books.SingleOrDefaultAsync(x => x.BookId == bookid);
            // Modifying original implementation
            var queryable = _context.Books.AsQueryable();
            return await queryable.SingleOrDefaultAsync(x => x.BookId == bookid);
        }

        public async Task<List<Book>> GetBooksByTitle(string title)
        {
            // Check: IEnumerable or IQueryable - Done
            var query = _context.Books.AsQueryable();
            return await query.Where(x => x.Title.ToLower().Contains(title.ToLower())).ToListAsync();
        }

        public async Task<List<Book>> GetBooksByDescription(string description)
        {
            var query = _context.Books.AsQueryable();
            return await query.Where(x => x.ShortDescr.ToLower().Contains(description.ToLower())).ToListAsync();
        }

        public async Task<IList<Book>> GetBooksByTitleAndDescription(string title, string description)
        {
            // Check: IEnumerable or IQueryable
            var query = _context.Books.AsQueryable();
            return await query.Where(x => x.Title.ToLower().Contains(title) &&
                x.ShortDescr.ToLower().Contains(description.ToLower())).ToListAsync();
        }

        public async void UpdateBookAsync(Book book)
        {
            _context.Update<Book>(book);
            await _context.SaveChangesAsync();
        }

        public async void AddBookAsync(Book book)
        {
            _context.Add(book);
            await _context.SaveChangesAsync();
        }
    }
}
