using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RAG_Simple.Models;
using RAG_Simple.Service;

namespace RAG_Simple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HandbookController : ControllerBase
    {
        private readonly HandbookPineconeService _handbookService;

        public HandbookController(HandbookPineconeService handbookService)
        {
            _handbookService = handbookService;
        }

        [HttpPost("sections")]
        public async Task<IActionResult> AddHandbookSections([FromBody] List<HandbookSection> sections)
        {
            try
            {
                var vectorIds = await _handbookService.UpsertHandbookContentAsync(sections);
                return Ok(new
                {
                    VectorIds = vectorIds,
                    Message = $"{sections.Count} handbook sections added successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPost("section")]
        public async Task<IActionResult> AddHandbookSection([FromBody] HandbookSection section)
        {
            try
            {
                var vectorId = await _handbookService.UpsertHandbookSectionAsync(section);
                return Ok(new { VectorId = vectorId, Message = "Handbook section added successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPost("qa")]
        public async Task<IActionResult> AddQAPair([FromBody] HandbookQA qa)
        {
            try
            {
                var vectorId = await _handbookService.UpsertHandbookQAPairAsync(qa);
                return Ok(new { VectorId = vectorId, Message = "Q&A pair added successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPost("bulk-upload")]
        public async Task<IActionResult> BulkUploadHandbook([FromBody] HandbookBulkUpload bulkUpload)
        {
            try
            {
                var results = new BulkUploadResult();

                if (bulkUpload.Sections?.Any() == true)
                {
                    results.SectionIds = await _handbookService.UpsertHandbookContentAsync(bulkUpload.Sections);
                }

                if (bulkUpload.QAPairs?.Any() == true)
                {
                    results.QAIds = new List<string>();
                    foreach (var qa in bulkUpload.QAPairs)
                    {
                        var qaId = await _handbookService.UpsertHandbookQAPairAsync(qa);
                        results.QAIds.Add(qaId);
                    }
                }

                return Ok(new
                {
                    Results = results,
                    Message = "Bulk upload completed successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchHandbook([FromBody] SearchRequest request)
        {
            try
            {
                var results = await _handbookService.SearchSimilarAsync(request.Query, request.TopK);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}
