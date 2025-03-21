using OnlineLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Models.Repositories.UnitOfWork;
using OnlineLibrary.Models.ReadingHistory;

namespace OnlineLibrary.Controllers
{
    [ApiController]
    [Route("api/reading-history")]
    public class ReadingHistoryController : Controller
    {
        private const int InternalServerErrorCode = 500;
        private const string InternalServerError = "Internal server error!";

        private readonly IUnitOfWork _unitOfWork;
        private readonly IReadingHistoryService _readingHistoryService;
        
        private LibraryDbContext _libraryDbContext;

        public ReadingHistoryController(IUnitOfWork unitOfWork, IReadingHistoryService readingHistoryService, LibraryDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _readingHistoryService = readingHistoryService;
            _libraryDbContext = dbContext;
        }

        [HttpGet]
        [ActionName(nameof(GetByUserId))]
        public async Task<IActionResult> GetByUserId([FromQuery] int userId, [FromQuery] int pageNumber = 1)
        {
            try
            {
                var readingHistory = await _unitOfWork.ReadingHistoryRepository.GetReadingHistoryOfUserAsync(userId, pageNumber);
                if (readingHistory == null) return NotFound("No reading history");

                return Json(readingHistory);
            }
            catch (Exception ex)
            {
                return StatusCode(InternalServerErrorCode, InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromQuery] int userId)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                if (user == null) return NotFound("User not found");

                var existingReadingHistory = await _unitOfWork.ReadingHistoryRepository.GetReadingHistoryOfUserWithoutBooksAsync(userId);
                if (existingReadingHistory != null) return Conflict("Reading history already exists for this user");

                var newReadingHistory = new ReadingHistoryModel
                {
                    UserId = userId,
                    AccessDate = DateTime.Now,
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


        [HttpPut("read")]
        public async Task<IActionResult> Read([FromQuery] int userId, [FromBody] ReadingHistoryDto readingHistoryDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {                   
                var updatedReadingHistory = await _readingHistoryService.UpdateReadingHistory(userId, readingHistoryDto);
                if (updatedReadingHistory == null) return NotFound();

                _unitOfWork.ReadingHistoryRepository.Update(updatedReadingHistory);
                await _unitOfWork.CommitAsync();
                 
                return Ok(updatedReadingHistory);
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
