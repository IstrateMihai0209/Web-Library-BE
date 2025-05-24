using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        public async Task<IActionResult> GetWishlistOfUser([FromQuery] string userId, [FromQuery] int pageNumber = 1)
        {
            try
            {
                var wishlist = await _unitOfWork.WishlistRepository.GetOrCreateUserWishlistAsync(userId, pageNumber);
                await _unitOfWork.CommitAsync();

                return Json(wishlist);
            }
            catch (Exception ex)
            {
                return StatusCode(InternalServerErrorCode, InternalServerError);
            }
        }

        [Authorize]
        [HttpPut("add-book")]
        public async Task<IActionResult> AddBookToWishlist([FromQuery] string userId, [FromBody] WishlistDto wishlistDto)
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

        [Authorize]
        [HttpPut("remove-book")]
        public async Task<IActionResult> RemoveBookFromWishlist([FromQuery] string userId, [FromQuery] int bookId)
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
        
        [Authorize]
        [HttpGet("is-book-in-wishlist")]
        public async Task<IActionResult> GetBookFromWishlist([FromQuery] string userId, [FromQuery] int bookId)
        {
            try
            {
                var isBookInWishlist = await _wishlistService.IsBookInWishlist(userId, bookId);
                if (!isBookInWishlist) return NoContent();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(InternalServerErrorCode, InternalServerError);
            }
        }
    }
}
