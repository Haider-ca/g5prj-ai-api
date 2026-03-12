namespace AiService.DTOs.Requests
{
    public class EvaluateRequestDto
    {
        public string Question { get; set; } = string.Empty;
        public string StudentAnswer { get; set; } = string.Empty;
    }
}