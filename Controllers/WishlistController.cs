using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Models;
using OnlineLibrary.Models.Book;
using OnlineLibrary.Models.Repositories.UnitOfWork;

namespace OnlineLibrary.Controllers
{
    [ApiController]
    [Route("api/wishlist")]
    public class WishlistController : Controller
    {
        private const int InternalServerErrorCode = 500;
        private const string InternalServerError = "Internal server error!";

        private readonly IUnitOfWork _unitOfWork;

        public WishlistController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{userId}")]
        [ActionName(nameof(GetWishlistOfUser))]
        public async Task<IActionResult> GetWishlistOfUser(int userId)
        {
            try
            {
                var wishlist = await _unitOfWork.WishlistRepository.GetUserWishlistAsync(userId);
                if (wishlist == null) return NotFound();

                return Json(wishlist);
            }
            catch (Exception ex)
            {
                return StatusCode(InternalServerErrorCode, InternalServerError);
            }
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> AddWishlistForUser(int userId)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                if (user == null) return NotFound("User not found");

                var existingWishlist = await _unitOfWork.WishlistRepository.GetUserWishlistAsync(userId);
                if (existingWishlist != null) return Conflict("Wishlist already exists for this user");

                var newWishlist = new WishlistModel
                {
                    UserId = userId,
                    Books = new List<BookModel>()
                };

                await _unitOfWork.WishlistRepository.AddAsync(newWishlist);
                await _unitOfWork.CommitAsync();

                return CreatedAtAction(
                    nameof(GetWishlistOfUser),
                    new { userId = user.Id },
                    newWishlist);
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

        [HttpPut("{wishlistId}")]
        public async Task<IActionResult> AddBookToWishlist([FromQuery] int bookId, int wishlistId)
        {
            try
            {
                var wishlist = await _unitOfWork.WishlistRepository.GetByIdAsync(wishlistId);
                if (wishlist == null) return NotFound("No wishlist found!");

                var book = await _unitOfWork.BookRepository.GetByIdAsync(bookId);
                if (book == null) return NotFound("No book found!");

                wishlist.Books.Add(book);
                _unitOfWork.WishlistRepository.Update(wishlist);

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

        [HttpDelete("{wishlistId}")]
        public async Task<IActionResult> RemoveBookFromWishlist([FromQuery] int bookId, int wishlistId)
        {
            try
            {
                var wishlist = await _unitOfWork.WishlistRepository.GetByIdAsync(wishlistId);
                if (wishlist == null) return NotFound("No wishlist found!");

                var book = await _unitOfWork.BookRepository.GetByIdAsync(bookId);
                if (book == null) return NotFound("No book found!");

                wishlist.Books.Remove(book);
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
