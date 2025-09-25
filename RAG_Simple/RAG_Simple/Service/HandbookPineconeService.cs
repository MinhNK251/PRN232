using RAG_Simple.Models;

namespace RAG_Simple.Service
{
    public class HandbookPineconeService : PineconeService
    {
        public HandbookPineconeService(string apiKey, string indexName = "test")
            : base(apiKey, indexName) { }

        public async Task<List<string>> UpsertHandbookContentAsync(List<HandbookSection> sections)
        {
            var vectorIds = new List<string>();

            foreach (var section in sections)
            {
                var vectorId = await UpsertHandbookSectionAsync(section);
                vectorIds.Add(vectorId);
            }

            return vectorIds;
        }

        public async Task<string> UpsertHandbookSectionAsync(HandbookSection section)
        {
            var metadata = new Dictionary<string, object>
            {
                ["section_id"] = section.SectionId,
                ["title"] = section.Title,
                ["category"] = section.Category,
                ["tags"] = section.Tags,
                ["importance"] = section.Importance,
                ["last_updated"] = section.LastUpdated.ToString("o"),
                ["version"] = section.Version
            };

            return await UpsertDocumentAsync(
                section.Content,
                section.SectionId,
                metadata
            );
        }

        public async Task<string> UpsertHandbookQAPairAsync(HandbookQA qa)
        {
            var metadata = new Dictionary<string, object>
            {
                ["qa_id"] = qa.QuestionId,
                ["question"] = qa.Question,
                ["category"] = qa.Category,
                ["difficulty"] = qa.Difficulty,
                ["related_topics"] = qa.RelatedTopics
            };

            // Use both question and answer for better retrieval
            var content = $"Question: {qa.Question}\nAnswer: {qa.Answer}";

            return await UpsertDocumentAsync(
                content,
                qa.QuestionId,
                metadata
            );
        }
    }
}
