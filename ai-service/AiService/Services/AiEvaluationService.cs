using AiService.Common;
using AiService.DTOs.Requests;
using AiService.DTOs.Responses;
using AiService.Interfaces.Clients;
using AiService.Interfaces.Services;
using AiService.Validators;
using System.Security.Claims;

namespace AiService.Services
{
    public class AiEvaluationService : IAiEvaluationService
    {
        private readonly IAiProviderClient _aiProviderClient;
        private readonly IAuthUsageClient _authUsageClient;
        private readonly EvaluateRequestValidator _validator;
        private readonly ILogger<AiEvaluationService> _logger;

        public AiEvaluationService(
            IAiProviderClient aiProviderClient,
            IAuthUsageClient authUsageClient,
            EvaluateRequestValidator validator,
            ILogger<AiEvaluationService> logger)
        {
            _aiProviderClient = aiProviderClient;
            _authUsageClient = authUsageClient;
            _validator = validator;
            _logger = logger;
        }

        public async Task<ServiceResult<EvaluateResponseDto>> EvaluateAsync(
            EvaluateRequestDto request,
            string bearerToken,
            ClaimsPrincipal user)
        {
            var validationError = _validator.Validate(request);
            if (validationError != null)
            {
                return ServiceResult<EvaluateResponseDto>.Fail(validationError, 400);
            }

            // var usageCheck = await _authUsageClient.CheckUsageAsync(bearerToken);
            // if (!usageCheck.Allowed)
            // {
            //     return ServiceResult<EvaluateResponseDto>.Fail(usageCheck.Message, 403);
            // }

            var remainingCalls = 20; // to be replaced with actual check from auth service above

            var aiResult = await _aiProviderClient.EvaluateAnswerAsync(request.Question, request.StudentAnswer);

            // var decrementResult = await _authUsageClient.DecrementUsageAsync(bearerToken);
            // if (!decrementResult.Success)
            // {
            //     _logger.LogWarning("AI evaluated successfully, but usage decrement failed.");
            //     return ServiceResult<EvaluateResponseDto>.Fail("Evaluation completed but usage update failed.", 500);
            // }


            // var response = new EvaluateResponseDto
            // {
            //     Score = aiResult.Score,
            //     Feedback = aiResult.Feedback,
            //     Model = aiResult.Model,
            //     RemainingCalls = decrementResult.RemainingCalls
            // };

            var response = new EvaluateResponseDto  // to be replaced with actual remaining calls from auth service above
                {
                    Score = aiResult.Score,
                    Feedback = aiResult.Feedback,
                    Model = aiResult.Model,
                    RemainingCalls = remainingCalls
              };

            return ServiceResult<EvaluateResponseDto>.Ok(response);
        }
    }
}