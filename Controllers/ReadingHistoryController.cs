using OnlineLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using OnlineLibrary.Models.Book;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Models.Repositories.UnitOfWork;

namespace OnlineLibrary.Controllers
{
    [ApiController]
    [Route("api/reading-history")]
    public class ReadingHistoryController : Controller
    {
        private const int InternalServerErrorCode = 500;
        private const string InternalServerError = "Internal server error!";

        private readonly IUnitOfWork _unitOfWork;

        public ReadingHistoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{userId}")]
        [ActionName(nameof(GetByUserId))]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            try
            {
                var readingHistory = await _unitOfWork.ReadingHistoryRepository.GetReadingHistoryOfUserAsync(userId);
                if (readingHistory == null) return NotFound("No reading history");

                return Json(readingHistory);
            }
            catch (Exception ex)
            {
                return StatusCode(InternalServerErrorCode, InternalServerError);
            }
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> Add(int userId)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                if (user == null) return NotFound("User not found");

                var existingReadingHistory = await _unitOfWork.ReadingHistoryRepository.GetReadingHistoryOfUserAsync(userId);
                if (existingReadingHistory != null) return Conflict("Reading history already exists for this user");

                var newReadingHistory = new ReadingHistoryModel
                {
                    UserId = userId,
                    AccessDate = DateTime.Now,
                    Books = new List<BookModel> { }
                };

                await _unitOfWork.ReadingHistoryRepository.AddAsync(newReadingHistory);
                await _unitOfWork.CommitAsync();

                return CreatedAtAction(
                    nameof(GetByUserId),
                    new { userId = user.Id },
                    newReadingHistory);
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

        [HttpDelete("{readingHistoryId}")]
        public async Task<IActionResult> Delete(int readingHistoryId)
        {
            try
            {
                var success = await _unitOfWork.ReadingHistoryRepository.DeleteAsync(readingHistoryId);
                if (!success) return NotFound();

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
