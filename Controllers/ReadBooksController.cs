using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Models.Book;
using OnlineLibrary.Models.ReadBooks;
using OnlineLibrary.Models.Repositories.UnitOfWork;
using OnlineLibrary.Models.Wishlist;

namespace OnlineLibrary.Controllers
{
    [ApiController]
    [Route("api/read-books")]
    public class ReadBooksController : Controller
    {
        private const int InternalServerErrorCode = 500;
        private const string InternalServerError = "Internal server error!";

        private readonly IUnitOfWork _unitOfWork;
        private readonly IReadBooksService _readBooksService;
        private readonly IWishlistService _wishlistService;

        public ReadBooksController(IUnitOfWork unitOfWork, IReadBooksService readBooksService, IWishlistService wishlistService)
        {
            _unitOfWork = unitOfWork;
            _readBooksService = readBooksService;
            _wishlistService = wishlistService;
        }


        [HttpGet]
        [ActionName(nameof(GetReadBooksOfUser))]
        public async Task<IActionResult> GetReadBooksOfUser([FromQuery] int userId, [FromQuery] int pageNumber = 1)
        {
            try
            {
                var readBooks = await _unitOfWork.ReadBooksRepository.GetReadBooksByUser(userId, pageNumber);
                if (readBooks == null) return NotFound();

                return Json(readBooks);
            }
            catch (Exception ex)
            {
                return StatusCode(InternalServerErrorCode, InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddReadBooksSectionForUser([FromQuery] int userId)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                if (user == null) return NotFound("User not found");

                var existingWishlist = await _unitOfWork.ReadBooksRepository.GetReadBooksSectionByUser(userId);
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

        [HttpPut("add")]
        public async Task<IActionResult> AddBookToReadSection([FromQuery] int userId, [FromBody] ReadBooksDto readBooksDto)
        {
            try
            {
                var updatedReadBooksSection = await _readBooksService.AddToReadBooks(userId, readBooksDto);
                if (updatedReadBooksSection == null) return NotFound();

                // Removing book from wishlist if it's marked as read
                var updatedWishlist = await _wishlistService.RemoveFromWishlist(userId, readBooksDto.Books.Last().Id);
                
                _unitOfWork.ReadBooksRepository.Update(updatedReadBooksSection);
                _unitOfWork.WishlistRepository.Update(updatedWishlist);
                await _unitOfWork.CommitAsync();

                return Ok(updatedReadBooksSection);
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

        [HttpPut("remove")]
        public async Task<IActionResult> RemoveBookFromReadSection([FromQuery] int userId, [FromQuery] int bookId)
        {
            try
            {
                var updatedReadBooksSection = await _readBooksService.RemoveFromReadBooks(userId, bookId);
                if (updatedReadBooksSection == null) return NotFound();
                
                _unitOfWork.ReadBooksRepository.Update(updatedReadBooksSection);
                await _unitOfWork.CommitAsync();

                return Ok(updatedReadBooksSection);
            }
            catch (Exception ex)
            {
                return StatusCode(InternalServerErrorCode, InternalServerError);
            }
        }

        [HttpGet("is-marked-as-read")]
        public async Task<IActionResult> IsMarkedAsRead([FromQuery] int userId, [FromQuery] int bookId)
        {
            try
            {
                var isBookMarkedAsRead = await _readBooksService.IsBookMarkedAsRead(userId, bookId);
                if (!isBookMarkedAsRead) return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(InternalServerErrorCode, InternalServerError);
            }
        }
    }
}
