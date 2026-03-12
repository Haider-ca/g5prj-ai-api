namespace AiService.DTOs.Responses
{
    public class EvaluateResponseDto
    {
        public int Score { get; set; }
        public string Feedback { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int RemainingCalls { get; set; }
    }
}