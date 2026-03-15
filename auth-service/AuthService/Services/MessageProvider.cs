namespace AuthService.Services;

public sealed class MessageProvider : IMessageProvider
{
    private readonly ITextFileProvider _textFileProvider;
    private readonly Dictionary<string, string> _messageFiles = new(StringComparer.OrdinalIgnoreCase)
    {
        ["AccountCreated"] = "Resources/Messages/account-created.txt",
        ["AdminRequired"] = "Resources/Messages/admin-required.txt",
        ["CurrentUserLoaded"] = "Resources/Messages/current-user-loaded.txt",
        ["EmailExists"] = "Resources/Messages/email-exists.txt",
        ["InvalidCredentials"] = "Resources/Messages/invalid-credentials.txt",
        ["LoginSuccessful"] = "Resources/Messages/login-successful.txt",
        ["Unauthorized"] = "Resources/Messages/unauthorized.txt",
        ["UsageDecremented"] = "Resources/Messages/usage-decremented.txt",
        ["UsageLoaded"] = "Resources/Messages/usage-loaded.txt",
        ["UsageLimitReached"] = "Resources/Messages/usage-limit-reached.txt",
        ["UsersLoaded"] = "Resources/Messages/users-loaded.txt",
        ["ValidationFailed"] = "Resources/Messages/validation-failed.txt"
    };

    public MessageProvider(ITextFileProvider textFileProvider)
    {
        _textFileProvider = textFileProvider;
    }

    public string Get(string key)
    {
        return _messageFiles.TryGetValue(key, out var path)
            ? _textFileProvider.ReadText(path)
            : string.Empty;
    }
}
