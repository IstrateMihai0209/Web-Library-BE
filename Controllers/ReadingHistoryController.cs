using Microsoft.AspNetCore.Mvc;
using OnlineLibrary.Models.Repositories.ReadingHistory;

namespace OnlineLibrary.Controllers
{
    [ApiController]
    public class ReadingHistoryController : ControllerBase
    {
        private readonly IReadingHistoryRepository _readingHistoryRepository;
    
        public ReadingHistoryController(IReadingHistoryRepository readingHistoryRepository)
        {
            _readingHistoryRepository = readingHistoryRepository;
        }

        [HttpGet]
        public Task<IActionResult> Get(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
