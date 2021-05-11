using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TechLibrary.Services;
using TechLibrary.Contracts.Requests;
using TechLibrary.Contracts.Responses;

namespace TechLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : Controller
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBookService _bookService;

        public BooksController(ILogger<BooksController> logger, IBookService bookService)
        {
            _logger = logger;
            _bookService = bookService;
        }

        /// <summary>
        /// Get all books
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Get all books");

            var books = await _bookService.GetBooksAsync();

            return Ok(books);
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetPaginated([FromQuery] GetAllBookRequest request)
        {
            _logger.LogInformation("Get all books with pagination");

            var books = await _bookService.GetBooksAsync(request.PageNumber, request.PageSize);

            return Ok(books);
        }

        /// <summary>
        /// Get book by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation($"Get book by id {id}");

            var book = await _bookService.GetBookByIdAsync(id);

            return Ok(book);
        }

        /// <summary>
        /// Search by title / description or both
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        // Assumption: User can search books by title or description or both
        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> Search([FromBody] SearchBooksRequest request)
        {
            _logger.LogInformation($"Search books {request.Title} {request.Description}");
            var searchBooksResponse = await _bookService.SearchBooks(request.Title, request.Description);
            return Ok(searchBooksResponse);
        }

        /// <summary>
        /// edit book
        /// </summary>
        /// <param name="editBookRequest"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> Edit([FromBody] EditBookRequest editBookRequest)
        {
            _logger.LogInformation($"Edit book with id: { editBookRequest.BookId }");
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid edit book request from client");
                return BadRequest("Invalid request object");
            }
            await _bookService.EditBook(editBookRequest);
            return Ok(new EditBookResponse { Status = true, Error = string.Empty });
        }

        /// <summary>
        /// add new book
        /// </summary>
        /// <param name="addBookRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] AddBookRequest addBookRequest)
        {
            _logger.LogInformation($"Add new book");
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid add book request from client");
                return BadRequest("Invalid request object");
            }
            await _bookService.AddBook(addBookRequest);
            return Ok("Add success");
        }
    }
}
