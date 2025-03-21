using Microsoft.AspNetCore.Mvc;
using OnlineLibrary.Models.Repositories.UnitOfWork;

namespace OnlineLibrary.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        
    }
}
