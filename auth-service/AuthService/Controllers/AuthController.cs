using AuthService.DTOs;
using AuthService.Options;
using AuthService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AuthService.Controllers;

[ApiController]
[Route("")]
public class AuthController : ControllerBase
{
    private const string AuthCookieName = "auth_token";
    private readonly IAuthService _authService;
    private readonly JwtOptions _jwtOptions;
    private readonly IWebHostEnvironment _environment;

    public AuthController(
        IAuthService authService,
        IOptions<JwtOptions> jwtOptions,
        IWebHostEnvironment environment)
    {
        _authService = authService;
        _jwtOptions = jwtOptions.Value;
        _environment = environment;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(MessageResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        var result = await _authService.RegisterAsync(request);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, new ErrorResponseDto { Message = result.Message });
        }

        return StatusCode(StatusCodes.Status201Created, new MessageResponseDto { Message = result.Message });
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, new ErrorResponseDto { Message = result.Message });
        }

        AppendAuthCookie(result.Data!.Token);
        return Ok(result.Data);
    }

    [HttpPost("logout")]
    [ProducesResponseType(typeof(MessageResponseDto), StatusCodes.Status200OK)]
    public IActionResult Logout()
    {
        Response.Cookies.Delete(AuthCookieName, BuildCookieOptions());
        return Ok(new MessageResponseDto { Message = "Logged out successfully." });
    }

    private void AppendAuthCookie(string token)
    {
        Response.Cookies.Append(AuthCookieName, token, BuildCookieOptions());
    }

    private CookieOptions BuildCookieOptions()
    {
        return new CookieOptions
        {
            HttpOnly = true,
            Secure = !_environment.IsDevelopment(),
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes),
            Path = "/"
        };
    }
}
