namespace RAG_Simple.Models
{
    public class SearchRequest
    {
        public string Query { get; set; }
        public int TopK { get; set; } = 5;
    }
}
