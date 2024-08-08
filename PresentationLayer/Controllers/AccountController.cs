using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserManagementAPI.Identity;
using UserManagementAPI.Model.ViewModel;
using UserManagementAPI.ViewModels;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, PhoneNumber = model.PhoneNumber };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User"); // Assegna il ruolo "User" per default
                return Ok(new { Message = "User registered successfully" });
            }

            return BadRequest(result.Errors);
        }

        return BadRequest(ModelState);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    var token = GenerateJwtToken(user);
                    return Ok(new { Token = token });
                }
            }
            return Unauthorized();
        }

        return BadRequest(ModelState);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountViewModel model)
    {
        var user = await _userManager.FindByIdAsync(model.Id);
        if (user == null)
        {
            return NotFound("User not found");
        }

        user.Email = model.Email;
        user.PhoneNumber = model.PhoneNumber;
        user.UserName = model.Email;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            return Ok(new { Message = "Account updated successfully" });
        }

        return BadRequest(result.Errors);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteAccount()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found");
        }

        var result = await _userManager.DeleteAsync(user);

        if (result.Succeeded)
        {
            return Ok(new { Message = "Account deleted successfully" });
        }

        return BadRequest(result.Errors);
    }


    private string GenerateJwtToken(ApplicationUser user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
