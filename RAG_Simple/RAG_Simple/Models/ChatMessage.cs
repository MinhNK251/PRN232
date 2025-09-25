using RAG_Simple.Services;

namespace RAG_Simple.Models
{
    public class ChatMessage
    {
        public ChatRole Role { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public enum ChatRole
    {
        User,
        Model
    }
}
