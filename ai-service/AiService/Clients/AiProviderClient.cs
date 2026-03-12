using AiService.Interfaces.Clients;

namespace AiService.Clients
{
    public class AiProviderClient : IAiProviderClient
    {
        private readonly ILogger<AiProviderClient> _logger;

        public AiProviderClient(ILogger<AiProviderClient> logger)
        {
            _logger = logger;
        }

        public async Task<(int Score, string Feedback, string Model)> EvaluateAnswerAsync(string question, string studentAnswer)
        {
            await Task.Delay(200);

            var score = studentAnswer.Length >= 80 ? 8 : 5;
            var feedback = score >= 8
                ? "Good answer. Add one concrete example to make it stronger."
                : "The answer is too brief. Add more detail and a clear example.";

            return (score, feedback, "mock-ai");
        }
    }
}