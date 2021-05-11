using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TechLibrary.Contracts.Requests;
using TechLibrary.Domain;
using TechLibrary.Models;
using TechLibrary.Repositories;

namespace TechLibrary.Services
{
    public interface IBookService
    {
        Task<List<BookResponse>> GetBooksAsync();
        Task<List<BookResponse>> GetBooksAsync(int pageNumber, int pageSize);
        Task<BookResponse> GetBookByIdAsync(int bookid);
        Task<IList<BookResponse>> SearchBooks(string title, string description);
        Task EditBook(EditBookRequest request);
        Task AddBook(AddBookRequest request);
    }

    public class BookService : IBookService
    {   
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;
        
        public BookService(IBookRepository repository, IMapper mapper, ILogger<BookService> logger)
        {
            _bookRepository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<BookResponse>> GetBooksAsync()
        {   
            try
            {
                var books = await _bookRepository.GetBooksAsync();
                var bookResponse = _mapper.Map<List<BookResponse>>(books);
                return bookResponse;
            }
            catch(Exception ex)
            {   
                _logger.LogError("Exception while getting all books", ex.Message);
                throw new ApplicationException(ex.Message);
            }
        }

        public async Task<List<BookResponse>> GetBooksAsync(int pageNumber, int pageSize)
        {
            try
            {
                var books = await _bookRepository.GetBooksAsync(pageNumber, pageSize);
                var bookResponse = _mapper.Map<List<BookResponse>>(books);
                return bookResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception while getting all books wih pageNumber {pageNumber}", ex.Message);
                throw new ApplicationException(ex.Message);
            }
        }

        public async Task<BookResponse> GetBookByIdAsync(int bookid)
        {
            // return await _dataContext.Books.SingleOrDefaultAsync(x => x.BookId == bookid);
            // Modifying original implementation
            try
            {
                var book = await _bookRepository.GetBookByIdAsync(bookid);
                var bookResponse = _mapper.Map<BookResponse>(book);
                return bookResponse;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Exception while getting book by id {bookid}", ex.Message);
                throw new ApplicationException(ex.Message);
            }
        }

        public async Task<IList<BookResponse>> SearchBooks(string title, string description)
        {   
            if (string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(description)) 
                return new List<BookResponse>();

            try
            {   
                // if description is empty search by title
                if (string.IsNullOrWhiteSpace(description))
                {
                    var booksData = await _bookRepository.GetBooksByTitle(title);
                    return _mapper.Map<List<BookResponse>>(booksData);
                }

                // if title is empty search by description
                if (string.IsNullOrWhiteSpace(title))
                {
                    var booksData = await _bookRepository.GetBooksByDescription(description);
                    return _mapper.Map<List<BookResponse>>(booksData);
                }

                var books = await _bookRepository.GetBooksByTitleAndDescription(title, description);
                return _mapper.Map<List<BookResponse>>(books);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception while search books", ex.Message);
                throw new ApplicationException(ex.Message);
            }
        }

        public async Task EditBook(EditBookRequest request)
        {
            if (request.BookId <= 0)
                throw new Exception("Invalid book id. Cross check input");
            
            Book book = await this._bookRepository.GetBookByIdAsync(request.BookId);
            if (book == null)
            {
                _logger.LogError("Exception while find book by Id", $"{ request.BookId }");
                throw new Exception("Book not found");
            }
                            
            try
            {   
                book.Title = request.Title;
                book.PublishedDate = request.PublishedDate;
                book.ThumbnailUrl = request.ThumbnailUrl;
                book.ShortDescr = request.ShortDescr;
                await Task.Run(() => _bookRepository.UpdateBookAsync(book));
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddBook(AddBookRequest request)
        {
            // NOTE: This is not complete, more work to do here
            try
            {
                Book book = new Book
                {
                    Title = request.Title,
                    ISBN = request.Isbn,
                    PublishedDate = request.PublishedDate,
                    ThumbnailUrl = request.ThumbnailUrl,
                    ShortDescr = request.ShortDescr
                };
                await Task.Run(() => _bookRepository.AddBookAsync(book));
            }
            catch(Exception ex)
            {
                _logger.LogError("Exception while adding book", ex.Message);
                throw new ApplicationException(ex.Message);
            }
        }
    }
}
