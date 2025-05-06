using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

        public ReadingHistoryController(
            IUnitOfWork unitOfWork,
            IReadingHistoryService readingHistoryService)
        {
            _unitOfWork = unitOfWork;
            _readingHistoryService = readingHistoryService;
        }

        [HttpGet]
        [ActionName(nameof(GetByUserId))]
        public async Task<IActionResult> GetByUserId([FromQuery] string userId, [FromQuery] int pageNumber = 1)
        {
            try
            {
                var readingHistory = await _unitOfWork.ReadingHistoryRepository.GetOrCreateReadingHistoryOfUserAsync(userId, pageNumber);
                if (readingHistory == null) return NotFound("No reading history");

                return Json(readingHistory);
            }
            catch (Exception ex)
            {
                return StatusCode(InternalServerErrorCode, InternalServerError);
            }
        }
        
        [Authorize]
        [HttpPut("read")]
        public async Task<IActionResult> Read([FromQuery] string userId, [FromBody] ReadingHistoryDto readingHistoryDto)
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

        [Authorize]
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
