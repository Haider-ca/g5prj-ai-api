namespace AiService.Interfaces.Clients
{
    public interface IAiProviderClient
    {
        Task<(int Score, string Feedback, string Model)> EvaluateAnswerAsync(string question, string studentAnswer);
    }
}