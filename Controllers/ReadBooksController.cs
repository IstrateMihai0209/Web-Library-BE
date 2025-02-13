using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Models.Book;
using OnlineLibrary.Models;
using OnlineLibrary.Models.Repositories.UnitOfWork;

namespace OnlineLibrary.Controllers
{
    [ApiController]
    [Route("api/read-books")]
    public class ReadBooksController : Controller
    {
        private const int InternalServerErrorCode = 500;
        private const string InternalServerError = "Internal server error!";

        private readonly IUnitOfWork _unitOfWork;

        public ReadBooksController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet("{userId}")]
        [ActionName(nameof(GetReadBooksOfUser))]
        public async Task<IActionResult> GetReadBooksOfUser(int userId)
        {
            try
            {
                var readBooks = await _unitOfWork.ReadBooksRepository.GetReadBooksByUser(userId);
                if (readBooks == null) return NotFound();

                return Json(readBooks);
            }
            catch (Exception ex)
            {
                return StatusCode(InternalServerErrorCode, InternalServerError);
            }
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> AddReadBooksSectionForUser(int userId)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                if (user == null) return NotFound("User not found");

                var existingWishlist = await _unitOfWork.ReadBooksRepository.GetReadBooksByUser(userId);
                if (existingWishlist != null) return Conflict("Read books section already exists for this user");

                var newReadBooksModel = new ReadBooksModel
                {
                    UserId = userId,
                    Books = new List<BookModel>()
                };

                await _unitOfWork.ReadBooksRepository.AddAsync(newReadBooksModel);
                await _unitOfWork.CommitAsync();

                return CreatedAtAction(
                    nameof(GetReadBooksOfUser),
                    new { userId = user.Id },
                    newReadBooksModel);
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

        [HttpPut("{readBooksId}")]
        public async Task<IActionResult> AddBookToWishlist([FromQuery] int bookId, int readBooksId)
        {
            try
            {
                var readBooksModel = await _unitOfWork.ReadBooksRepository.GetByIdAsync(readBooksId);
                if (readBooksModel == null) return NotFound("No read books section found!");

                var book = await _unitOfWork.BookRepository.GetByIdAsync(bookId);
                if (book == null) return NotFound("No book found!");

                readBooksModel.Books.Add(book);
                _unitOfWork.ReadBooksRepository.Update(readBooksModel);

                await _unitOfWork.CommitAsync();
                return Ok(book);
            }
            catch (DbUpdateException ex)
            {
                return Conflict("Entity was modified by another user!");
            }
            catch (Exception ex)
            {
                return StatusCode(InternalServerErrorCode, InternalServerError);
            }
        }

        [HttpDelete("{readBooksId}")]
        public async Task<IActionResult> RemoveBookFromWishlist([FromQuery] int bookId, int readBooksId)
        {
            try
            {
                var readBooksModel = await _unitOfWork.ReadBooksRepository.GetByIdAsync(readBooksId);
                if (readBooksModel == null) return NotFound("No read books section found!");

                var book = await _unitOfWork.BookRepository.GetByIdAsync(bookId);
                if (book == null) return NotFound("No book found!");

                readBooksModel.Books.Remove(book);
                await _unitOfWork.CommitAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(InternalServerErrorCode, InternalServerError);
            }
        }
    }
}
