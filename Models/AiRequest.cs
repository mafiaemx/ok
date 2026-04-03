namespace ok.Models
{
    public class AiRequest
    {
        public string Prompt { get; set; } = string.Empty;
        public string? Model { get; set; }
    }
}