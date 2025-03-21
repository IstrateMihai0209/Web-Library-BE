using OnlineLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using OnlineLibrary.Models.Repositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace OnlineLibrary.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : Controller
    {
        private const int InternalServerErrorCode = 500;
        private const string InternalServerError = "Internal server error!";

        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [ActionName(nameof(GetById))]
        public async Task<IActionResult> GetById([FromQuery] int userId)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                if (user == null) return NotFound();

                return Json(user);
            }
            catch (Exception ex)
            {
                return StatusCode(InternalServerErrorCode, InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UserModel user)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var createdUser = await _unitOfWork.UserRepository.AddAsync(user);
                await _unitOfWork.CommitAsync();

                return CreatedAtAction(
                    nameof(GetById),
                    new { userId = createdUser.Id },
                    user);
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

        //public IActionResult Login(string returnUrl)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<IActionResult> Login(LoginModel loginModel)
        //{
        //    throw new NotImplementedException();
        //}

        //public IActionResult LoginWithGoogle(string returnUrl)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<IActionResult> GoogleLoginCallback()
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<IActionResult> Logout()
        //{
        //    throw new NotImplementedException();
        //}

        //[HttpPut]
        //public Task<IActionResult> Update(int userId)
        //{
        //    throw new NotImplementedException();
        //}

        //[HttpDelete]
        //public Task<IActionResult> Delete(int userId)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
