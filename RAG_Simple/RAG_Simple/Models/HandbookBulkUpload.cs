using RAG_Simple.Service;

namespace RAG_Simple.Models
{
    public class HandbookBulkUpload
    {
        public List<HandbookSection>? Sections { get; set; }
        public List<HandbookQA>? QAPairs { get; set; }
    }
}
