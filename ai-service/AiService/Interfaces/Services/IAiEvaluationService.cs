using AiService.Common;
using AiService.DTOs.Requests;
using AiService.DTOs.Responses;
using System.Security.Claims;

namespace AiService.Interfaces.Services
{
    public interface IAiEvaluationService
    {
        Task<ServiceResult<EvaluateResponseDto>> EvaluateAsync(
            EvaluateRequestDto request,
            string bearerToken,
            ClaimsPrincipal user);
    }
}