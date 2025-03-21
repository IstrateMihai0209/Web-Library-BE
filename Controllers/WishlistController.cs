using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Models.Wishlist;
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
        private readonly IWishlistService _wishlistService;

        public WishlistController(IUnitOfWork unitOfWork, IWishlistService wishlistService)
        {
            _unitOfWork = unitOfWork;
            _wishlistService = wishlistService;
        }

        [HttpGet]
        [ActionName(nameof(GetWishlistOfUser))]
        public async Task<IActionResult> GetWishlistOfUser([FromQuery] int userId, [FromQuery] int pageNumber = 1)
        {
            try
            {
                var wishlist = await _unitOfWork.WishlistRepository.GetUserWishlistAsync(userId, pageNumber);
                if (wishlist == null) return NotFound();

                return Json(wishlist);
            }
            catch (Exception ex)
            {
                return StatusCode(InternalServerErrorCode, InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddWishlistForUser([FromQuery] int userId)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                if (user == null) return NotFound("User not found");

                var existingWishlist = await _unitOfWork.WishlistRepository.GetUserWishlistWithNoBooksAsync(userId);
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

        [HttpPut("add-book")]
        public async Task<IActionResult> AddBookToWishlist([FromQuery] int userId, [FromBody] WishlistDto wishlistDto)
        {
            try
            {
                var updatedWishlist = await _wishlistService.AddToWishlist(userId, wishlistDto);
                if (updatedWishlist == null) return NotFound();

                _unitOfWork.WishlistRepository.Update(updatedWishlist);
                await _unitOfWork.CommitAsync();

                return Ok(updatedWishlist);
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

        [HttpPut("remove-book")]
        public async Task<IActionResult> RemoveBookFromWishlist([FromQuery] int userId, [FromQuery] int bookId)
        {
            try
            {
                var updatedWishlist = await _wishlistService.RemoveFromWishlist(userId, bookId);
                if (updatedWishlist == null) return NotFound();
                
                _unitOfWork.WishlistRepository.Update(updatedWishlist);
                await _unitOfWork.CommitAsync();

                return Ok(updatedWishlist);
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
        
        [HttpGet("is-book-in-wishlist")]
        public async Task<IActionResult> GetBookFromWishlist([FromQuery] int userId, [FromQuery] int bookId)
        {
            try
            {
                var isBookInWishlist = await _wishlistService.IsBookInWishlist(userId, bookId);
                if (!isBookInWishlist) return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(InternalServerErrorCode, InternalServerError);
            }
        }
    }
}
