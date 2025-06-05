using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineLibrary.Models;

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
        
        [Authorize]
        [HttpPut("change-username")]
        public async Task<IActionResult> UpdateUsername([FromBody] UsernameModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return NotFound(new { Message = "User not found!" });

                var existingUser = await _userManager.FindByNameAsync(model.Username);
                if (existingUser != null && existingUser.Id != user.Id)
                    return BadRequest(new { Message = "Username already taken!" });

                user.UserName = model.Username;
                user.NormalizedUserName = _userManager.NormalizeName(model.Username);

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    return BadRequest(new { Message = result.Errors });

                return Ok(new { Message = "Username updated succesfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(InternalServerErrorCode, InternalServerError);
            }
        }
    }
}