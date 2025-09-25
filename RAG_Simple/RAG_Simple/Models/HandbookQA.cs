namespace RAG_Simple.Models
{
    public class HandbookQA
    {
        public string QuestionId { get; set; } = Guid.NewGuid().ToString();
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public string Category { get; set; } = "General";
        public string Difficulty { get; set; } = "Medium"; // Easy, Medium, Hard
        public List<string> RelatedTopics { get; set; } = new List<string>();
    }
}
