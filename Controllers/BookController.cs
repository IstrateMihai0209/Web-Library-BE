using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Models;
using OnlineLibrary.Models.Repositories.Book;
using OnlineLibrary.Models.Repositories.Category;
using OnlineLibrary.Models.Repositories.UnitOfWork;

namespace OnlineLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : Controller
    {
        private const int InternalServerErrorCode = 500;
        private const string InternalServerError = "Internal server error!";

        // Remove these if IUnitOfWork does their job
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;

        private readonly IUnitOfWork _unitOfWork;

        public BookController(IBookRepository bookRepository, ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{bookId}")]
        public async Task<IActionResult> GetById(int bookId)
        {
            try
            {
                var book = await _bookRepository.GetByIdAsync(bookId);
                if (book == null) return NotFound();

                return Json(book);
            }
            catch (Exception ex)
            {
                return StatusCode(InternalServerErrorCode, InternalServerError);
            }
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> ListByCategory(int categoryId)
        {
            try
            {
                var books = await _bookRepository.GetBooksByCategoryAsync(categoryId);
                if (books == null) return NotFound();

                return Json(books);
            }
            catch (Exception ex)
            {
                return StatusCode(InternalServerErrorCode, InternalServerError);
            }
        }

        [HttpGet("uploader/{userId}")]
        public async Task<IActionResult> ListByUploader(int userId)
        {
            try
            {
                var books = await _bookRepository.GetBooksOfUploaderAsync(userId);
                if (books == null) return NotFound();

                return Json(books);
            }
            catch (Exception ex)
            {
                return StatusCode(InternalServerErrorCode, InternalServerError);
            }
        }

        [HttpGet("details/{bookId}")]
        public async Task<IActionResult> Details(int bookId)
        {
            try
            {
                var book = await _bookRepository.GetByIdAsync(bookId);
                if (book == null) return NotFound();

                return Json(book);
            }
            catch (Exception ex)
            {
                return StatusCode(InternalServerErrorCode, InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchQuery)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> Read(int bookId)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{bookId}")]
        public async Task<IActionResult> Delete(int bookId)
        {
            try
            {
                var success = await _bookRepository.DeleteAsync(bookId);
                if (!success) return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(InternalServerErrorCode, InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromBody] BookModel book)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var createdBook = await _unitOfWork.BookRepository.AddAsync(book);
                await _unitOfWork.CommitAsync();

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = createdBook.Id },
                    book);
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

        [HttpPut("{bookId}")]
        public async Task<IActionResult> Edit(int bookId, [FromBody] BookModel updatedBook)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (bookId != updatedBook.Id) return BadRequest("ID mismatch");

            try
            {
                var existingBook = await _bookRepository.UpdateAsync(updatedBook);
                if(existingBook == null) return NotFound();

                return Ok(existingBook);
            }
            catch (Exception ex)
            {
                return StatusCode(InternalServerErrorCode, InternalServerError);
            }
        }
    }
}
