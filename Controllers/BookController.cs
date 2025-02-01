using Microsoft.AspNetCore.Mvc;
using OnlineLibrary.Models;
using OnlineLibrary.Models.Repositories.Book;
using OnlineLibrary.Models.Repositories.Category;

namespace OnlineLibrary.Controllers
{
    [ApiController]
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BookController(IBookRepository bookRepository, ICategoryRepository categoryRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> List(string category)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> List(int userId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int bookId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchQuery)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> Read(int bookId)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int bookId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            var newBook = new BookModel();
            newBook.Id = 1000;
            newBook.Title = "Capra cu trei iezi";
            newBook.FilePath = "aaa";
            newBook.UploadedAt = DateTime.Now;
            newBook.UserId = 1000;


            await _bookRepository.AddAsync(newBook);
            return Json(newBook);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int bookId)
        {
            throw new NotImplementedException();
        }
    }
}
