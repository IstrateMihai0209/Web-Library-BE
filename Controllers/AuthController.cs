using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OnlineLibrary.Models;
using Exception = System.Exception;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace OnlineLibrary.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IEmailSender _emailSender;
    
    public AuthController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IConfiguration configuration,
        IEmailSender emailSender)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _emailSender = emailSender;
    }

    [BlockAuthenticated]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Errors = ModelState.Values.SelectMany(v => v.Errors) });

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized(new { message = "Invalid login attempt!" });

            // Email confirmation
            // if (!await _userManager.IsEmailConfirmedAsync(user))
            //      return StatusCode(StatusCodes.Status403Forbidden, new { message = "Email is not confirmed!" });
            
            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                true,
                false);

            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var token = GenerateJwtToken(user);
                
                Response.Cookies.Delete("auth-token");
                
                Response.Cookies.Append("auth-token", token, new CookieOptions()
                {
                    HttpOnly = true, // Prevent XSS
                    Secure = true, // Only send over HTTPS
                    SameSite = SameSiteMode.Strict, // Warning!!!: Set to SameSiteMode.Strict to Prevent CSRF when frontend is on the same domain
                    Expires = DateTime.UtcNow.AddDays(1)
                });
                
                return Ok(new { Message = "Login successful!" });
            }
            
            if (result.RequiresTwoFactor)
                return StatusCode(StatusCodes.Status402PaymentRequired, new { Error = "2FA required!"});

            if (result.IsLockedOut)
                return StatusCode(StatusCodes.Status423Locked, new { Error = "Account locked out!" });
            
            if (result.IsNotAllowed)
                return StatusCode(StatusCodes.Status403Forbidden, new { Error = "Login not allowed!" });
            
            return Unauthorized(new { Error = "Invalid login attempt!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
        }
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        Response.Cookies.Delete("auth-token");
        return Ok(new { message = "Logged out successfully!" });
    }
    
    [BlockAuthenticated]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Errors = ModelState.Values.SelectMany(v => v.Errors) });

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            
            if (existingUser != null)
                return BadRequest(new { Error = "User with this email address already exists!" });

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Email confirmation
                // var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                // var confirmationLink = Url.Action(
                //     "ConfirmEmail",
                //     "Auth",
                //     new { userId = user.Id, token = confirmationToken},
                //     Request.Scheme);
                //
                // await _emailSender.SendEmailAsync(
                //     model.Email,
                //     "Confirm your email",
                //     $"Please confirm your account by clicking this link: {confirmationLink}");

                return Ok(new { Message = "Registration Successful", UserId = user.Id });
            }

            // Format Identity errors
            var errors = result.Errors.Select(e => new
            {
                Code = e.Code,
                Description = e.Description
            });

            return BadRequest(new { Errors = errors });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
        }
    }

    [Authorize]
    [HttpGet("userinfo")] 
    public async Task<IActionResult> GetUserInfo()
    {
        if (!User.Identity.IsAuthenticated)
            return Ok(new { isAuthenticated = false });

        var user = await _userManager.GetUserAsync(User);
        return Ok(new
        {
            isAuthenticated = true,
            user.Id,
            user.Email,
            user.UserName,
            EmailConfirmed = user.EmailConfirmed,
            Roles = await _userManager.GetRolesAsync(user)
        });
    }

    [BlockAuthenticated]
    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
                return BadRequest("Invalid parameters");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found!");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
                return Ok("Email confirmed successfully! You can now login.");

            return BadRequest("Error confirming email!");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
        }
    }

    [BlockAuthenticated]
    [HttpPost]
    public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
            return NotFound("User not found!");

        if (await _userManager.IsEmailConfirmedAsync(user))
            return BadRequest("Email is already confirmed!");

        var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmationLink = Url.Action(
            "ConfirmEmail",
            "Auth",
            new { userId = user.Id, token = confirmationToken }, Request.Scheme);

        await _emailSender.SendEmailAsync(
            model.Email,
            "Confirm your email",
            $"Please confirm your account by clicking this link: {confirmationLink}");
        
        return Ok(new { message = "Confirmation email resent successfully!" });
    }

    [BlockAuthenticated]
    [HttpGet("external-login")]
    public IActionResult ExternalLogin(string provider)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(provider))
                return BadRequest("Authentication provider not specified!");

            if (!IsValidProvider(provider))
                return BadRequest("Invalid authentication provider!");

            var redirectUrl = Url.Action("ExternalLoginCallback", "Auth", null, Request.Scheme);
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
        }
    }

    [HttpGet("signin-google")]
    public async Task<IActionResult> ExternalLoginCallback()
    {
        try
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded)
                return BadRequest("Google authentication failed!");

            // Extract claims
            var email = result.Principal.FindFirstValue(ClaimTypes.Email);
            var userId = result.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
            
            // Find or create user
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new IdentityUser { UserName = email.GetSubstringUntilCharacter('@'), Email = email, EmailConfirmed = true };
                await _userManager.CreateAsync(user);
            }
            
            // Sign in user
            await _signInManager.SignInAsync(user, isPersistent: false);
            
            // Set cookie and redirect
            Response.Cookies.Append("auth-token", GenerateJwtToken(user), new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(1)
            });

            return Redirect(_configuration["Frontend:Url"]);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
        }
    }
    
    private string GenerateJwtToken(IdentityUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim("auth_time", DateTime.UtcNow.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private bool IsValidProvider(string provider)
    {
        return _signInManager.GetExternalAuthenticationSchemesAsync()
            .Result.Any(s => s.Name == provider);
    }
}