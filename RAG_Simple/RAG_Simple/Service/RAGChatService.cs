using RAG_Simple.Models;
using System.Collections.Concurrent;

namespace RAG_Simple.Services
{
    public interface IRAGChatService
    {
        Task<ChatResponse> ChatAsync(string message, string sessionId = "default", int topK = 5);
        Task ClearConversationHistoryAsync(string sessionId);
    }

    public class RAGChatService : IRAGChatService
    {
        private readonly IGeminiService _geminiService;
        private readonly ConcurrentDictionary<string, List<ChatMessage>> _conversationHistory;
        private readonly PineconeService _pineconeService;
        public RAGChatService(IGeminiService geminiService, PineconeService pineconeService)
        {
            _geminiService = geminiService;
            _conversationHistory = new ConcurrentDictionary<string, List<ChatMessage>>();
            _pineconeService = pineconeService;
        }

        public async Task<ChatResponse> ChatAsync(string message, string sessionId = "default", int topK = 5)
        {
            // Get or create conversation history
            if (!_conversationHistory.TryGetValue(sessionId, out var history))
            {
                history = new List<ChatMessage>();
                _conversationHistory[sessionId] = history;
            }

            // 1. Search for relevant context
            var searchResults = await _pineconeService.SearchSimilarAsync(message, topK);
            var contextSnippets = searchResults.Select(r => r.Text).ToList();

            // 2. Generate response using Gemini with context
            var response = await _geminiService.GenerateResponseAsync(message, contextSnippets);

            // 3. Update conversation history
            history.Add(new ChatMessage { Role = ChatRole.User, Content = message });
            history.Add(new ChatMessage { Role = ChatRole.Model, Content = response });

            // Keep only last 10 messages to avoid context overflow
            if (history.Count > 10)
            {
                history = history.Skip(history.Count - 10).ToList();
                _conversationHistory[sessionId] = history;
            }

            return new ChatResponse
            {
                Answer = response,
                ContextSnippets = contextSnippets,
                SessionId = sessionId,
                ConversationHistory = history
            };
        }

        public Task ClearConversationHistoryAsync(string sessionId)
        {
            _conversationHistory.TryRemove(sessionId, out _);
            return Task.CompletedTask;
        }
    }
}