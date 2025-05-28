using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineLibrary.Models.Book;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Models.Repositories.UnitOfWork;
using OnlineLibrary.Storage;

namespace OnlineLibrary.Controllers
{
    [ApiController]
    [Route("api/book")]
    public class BookController : Controller
    {
        private const int InternalServerErrorCode = 500;
        private const string InternalServerError = "Internal server error!";

        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookService _bookService;
        private readonly IBookStorageService _bookStorageService;

        public BookController(IUnitOfWork unitOfWork, IBookService bookService, IBookStorageService bookStorageService)
        {
            _unitOfWork = unitOfWork;
            _bookService = bookService;
            _bookStorageService = bookStorageService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] BookDto book)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var bookModel = _bookService.CreateBookModel(book);
                await _bookStorageService.UploadBook(book.CoverImage, book.TextFile);
                
                var createdBook = await _unitOfWork.BookRepository.AddAsync(bookModel);
                await _unitOfWork.CommitAsync();
                
                return CreatedAtAction(
                    nameof(GetById),
                    new { bookId = createdBook.Id },
                    createdBook);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(InternalServerErrorCode, "A database error occured!");
            }
            catch (Exception ex)
            {
                return StatusCode(InternalServerErrorCode, InternalServerError);
            }
        }

        [HttpGet]
        [ActionName(nameof(GetById))]
        public async Task<IActionResult> GetById([FromQuery] int bookId)
        {
            try
            {
                var book = await _unitOfWork.BookRepository.GetByIdAsync(bookId);
                if (book == null) return NotFound();

                _bookService.IncreaseBookPopularity(book);
                _unitOfWork.BookRepository.Update(book);
                await _unitOfWork.CommitAsync();

                return Json(book);
            }
            catch (Exception ex)
            {
                return StatusCode(InternalServerErrorCode, InternalServerError);
            }
        }

        [HttpGet("uploader")]
        public async Task<IActionResult> ListByUploader([FromQuery] string userId, [FromQuery] int pageNumber)
        {
            try
            {
                var books = await _unitOfWork.BookRepository.GetBooksOfUploaderAsync(userId, pageNumber);
                if (books == null) return NotFound();

                return Json(books);
            }
            catch (Exception ex)
            {
                return StatusCode(InternalServerErrorCode, InternalServerError);
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Edit([FromQuery] int bookId, [FromBody] BookDto bookDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var updatedBook = await _bookService.UpdateBook(bookId, bookDto);
                if(updatedBook == null) return NotFound();

                _unitOfWork.BookRepository.Update(updatedBook);

                await _unitOfWork.CommitAsync();
                return Ok(updatedBook);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Conflict("Entity was modified by another user!");
            }
            catch (Exception ex)
            {
                return StatusCode(InternalServerErrorCode, InternalServerError);
            }
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int bookId)
        {
            try
            {
                var book = await _unitOfWork.BookRepository.GetByIdAsync(bookId);
                var success = await _unitOfWork.BookRepository.DeleteAsync(bookId);
                if (!success) return NotFound();

                await _bookStorageService.DeleteBook(book.Title.ToLower());
                
                await _unitOfWork.CommitAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(InternalServerErrorCode, InternalServerError);
            }
        }

        [HttpGet("top-popular")]
        public async Task<IActionResult> GetNextByPopularity([FromQuery] int pageNumber = 1)
        {
            try
            {
                var books = await _unitOfWork.BookRepository.GetTopPopularBooksAsync(pageNumber);
                if (books == null) return NotFound();

                return Json(books);
            }
            catch (Exception ex)
            {
                return StatusCode(InternalServerErrorCode, InternalServerError);
            }
        }

        [HttpGet("similar-books")]
        public async Task<IActionResult> GetSimilarBooks([FromQuery] int id)
        {
            try
            {
                var currentBook = await _unitOfWork.BookRepository.GetByIdAsync(id);
                if (currentBook == null) return NotFound();
                
                var similarBooks = await _unitOfWork.BookRepository.GetSimilarBooksAsync(currentBook);
                if(similarBooks.Count() == 0) return NoContent();

                return Ok(similarBooks);
            }
            catch (Exception e)
            {
                return StatusCode(InternalServerErrorCode, e);
            }
        }
        
        [HttpPost("search")]
        public async Task<IActionResult> Search([FromQuery] string? searchQuery, [FromBody] Dictionary<string, List<string>> filters, [FromQuery] int pageNumber = 1)
        {
            try
            {
                var books = await _unitOfWork.BookRepository.SearchBooks(searchQuery, filters, pageNumber);
                if (books == null || books.Count() == 0) return NoContent();
                return Ok(books);
            }
            catch (Exception e)
            {
                return StatusCode(InternalServerErrorCode, InternalServerError);
            }
        }
    }
}
