namespace RAG_Simple.Models
{
    public class HandbookSection
    {
        public string SectionId { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Category { get; set; } = "General";
        public List<string> Tags { get; set; } = new List<string>();
        public string Importance { get; set; } = "Medium"; // Low, Medium, High
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public string Version { get; set; } = "1.0";
    }
}
