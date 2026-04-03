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

    public AuthController(
        IAuthService authService,
        IOptions<JwtOptions> jwtOptions)
    {
        _authService = authService;
        _jwtOptions = jwtOptions.Value;
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

        Response.Cookies.Append(AuthCookieName, result.Data!.Token, BuildCookieOptions());
        return Ok(result.Data);
    }

    [HttpPost("logout")]
    [ProducesResponseType(typeof(MessageResponseDto), StatusCodes.Status200OK)]
    public IActionResult Logout()
    {
        Response.Cookies.Delete(AuthCookieName, BuildDeletionCookieOptions());
        return Ok(new MessageResponseDto { Message = "Logged out successfully." });
    }

    private CookieOptions BuildCookieOptions()
    {
        var isLocalHost = string.Equals(Request.Host.Host, "localhost", StringComparison.OrdinalIgnoreCase)
            || string.Equals(Request.Host.Host, "127.0.0.1", StringComparison.OrdinalIgnoreCase);
        var useSecureCookie = !isLocalHost;

        return new CookieOptions
        {
            HttpOnly = true,
            Secure = useSecureCookie,
            SameSite = useSecureCookie ? SameSiteMode.None : SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes),
            Path = "/"
        };
    }

    private CookieOptions BuildDeletionCookieOptions()
    {
        var cookieOptions = BuildCookieOptions();
        cookieOptions.Expires = null;
        return cookieOptions;
    }
}
