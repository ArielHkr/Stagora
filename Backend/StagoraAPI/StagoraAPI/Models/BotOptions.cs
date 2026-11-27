namespace StagoraAPI.Models
{
    public class BotOptions
    {
        public string Model { get; set; } = "openai/gpt-oss-120b";
        public int MaxTokens { get; set; } = 1024;
        public int MaxHistory { get; set; } = 10;

        public const string Section = "Bot";

        public string Endpoint { get; set; } = "https://api.groq.com/openai/v1";

    }
}
