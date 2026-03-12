namespace AiService.Interfaces.Clients
{
    public interface IAuthUsageClient
    {
        Task<(bool Allowed, int RemainingCalls, string Message)> CheckUsageAsync(string bearerToken);
        Task<(bool Success, int RemainingCalls, string Message)> DecrementUsageAsync(string bearerToken);
    }
}