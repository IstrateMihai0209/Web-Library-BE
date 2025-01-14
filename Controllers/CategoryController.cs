using Microsoft.AspNetCore.Mvc;
using OnlineLibrary.Models.Repositories.Category;

namespace OnlineLibrary.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public Task<IActionResult> Get(int categoryId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public Task<IActionResult> Add()
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public Task<IActionResult> Delete(int categoryId)
        {
            throw new NotImplementedException();
        }
    }
}
