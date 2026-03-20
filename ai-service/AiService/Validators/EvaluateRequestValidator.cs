using AiService.DTOs.Requests;

namespace AiService.Validators
{
    public class EvaluateRequestValidator
    {
        public string? Validate(EvaluateRequestDto request)
        {
            if (request == null)
                return "Request is required.";

            if (string.IsNullOrWhiteSpace(request.Question))
                return "Question is required.";

            if (string.IsNullOrWhiteSpace(request.StudentAnswer))
                return "Student answer is required.";

            if (request.Question.Length > 500)
                return "Question is too long.";

            if (request.StudentAnswer.Length > 3000)
                return "Student answer is too long.";

            return null;
        }
    }
}