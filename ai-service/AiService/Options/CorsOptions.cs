namespace AiService.Options
{
    public class CorsOptions
    {
        public const string SectionName = "Cors";
        public string AllowedOrigin { get; set; } = string.Empty;
        public List<string> AllowedOrigins { get; set; } = new List<string>();
    }
}