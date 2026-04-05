using AiService.DTOs.Requests;
using AiService.DTOs.Responses;
using AiService.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AiController : ControllerBase
    {
        private readonly IAiEvaluationService _aiEvaluationService;

        public AiController(IAiEvaluationService aiEvaluationService)
        {
            _aiEvaluationService = aiEvaluationService;
        }

        [HttpPost("evaluate")]
        [ProducesResponseType(typeof(EvaluateResponseDto), 200)]
        [ProducesResponseType(typeof(ErrorResponseDto), 400)]
        [ProducesResponseType(typeof(ErrorResponseDto), 403)]
        [ProducesResponseType(typeof(ErrorResponseDto), 500)]
        public async Task<IActionResult> Evaluate([FromBody] EvaluateRequestDto request)
        {
            var accessToken = Request.Headers.Authorization.ToString();
            if (string.IsNullOrWhiteSpace(accessToken) &&
                Request.Cookies.TryGetValue("auth_token", out var cookieToken))
            {
                accessToken = cookieToken;
            }

            var result = await _aiEvaluationService.EvaluateAsync(request, accessToken, User);

            if (!result.Success)
            {
                return StatusCode(result.StatusCode, new ErrorResponseDto
                {
                    Message = result.ErrorMessage
                });
            }

            return Ok(result.Data);
        }
    }
}
