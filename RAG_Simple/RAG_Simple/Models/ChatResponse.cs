namespace RAG_Simple.Models
{
    public class ChatResponse
    {
        public string Answer { get; set; } = string.Empty;
        public List<string> ContextSnippets { get; set; } = new List<string>();
        public string SessionId { get; set; } = string.Empty;
        public List<ChatMessage> ConversationHistory { get; set; } = new List<ChatMessage>();
    }
}
