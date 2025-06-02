using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace OnlineLibrary.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : Controller
    {
        private const int InternalServerErrorCode = 500;
        private const string InternalServerError = "Internal server error!";
        
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetUserNameById(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return NotFound();
                
                return Ok(user.UserName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(InternalServerErrorCode, InternalServerError);
            }
        }
    }
}