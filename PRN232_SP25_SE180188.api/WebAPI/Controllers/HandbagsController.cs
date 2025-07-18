using BLL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace WebAPI.Controllers
{
    [Route("api/handbags")]
    [ApiController]
    public class HandbagsController : ODataController
    {
        private readonly IHandbagService _service;

        public HandbagsController(IHandbagService service)
        {
            _service = service;
        }

        [EnableQuery]
        [HttpGet]
        [Authorize(Roles = "administrator,moderator,developer,member")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "administrator,moderator,developer,member")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
                return NotFound(new { errorCode = "HB40401", message = "Handbag not found" });

            return Ok(item);
        }

        [HttpPost]
        [Authorize(Roles = "administrator,moderator")]
        public async Task<IActionResult> Create([FromBody] Handbag handbag)
        {
            var newId = await _service.GetAllAsync().ContinueWith(t => t.Result.Max(h => h.HandbagId) + 1);
            handbag.HandbagId = newId;
            var (isSuccess, errorCode, errorMsg) = await _service.AddAsync(handbag);
            if (!isSuccess)
                return BadRequest(new { errorCode, message = errorMsg });

            return StatusCode(201);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "administrator,moderator")]
        public async Task<IActionResult> Update(int id, [FromBody] Handbag handbag)
        {
            handbag.HandbagId = id;
            var (isSuccess, errorCode, errorMsg) = await _service.UpdateAsync(handbag);
            if (!isSuccess)
            {
                if (errorCode == "HB40401")
                    return NotFound(new { errorCode, message = errorMsg });
                return BadRequest(new { errorCode, message = errorMsg });
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "administrator,moderator")]
        public async Task<IActionResult> Delete(int id)
        {
            var (isSuccess, errorCode, errorMsg) = await _service.DeleteAsync(id);
            if (!isSuccess)
                return NotFound(new { errorCode, message = errorMsg });

            return Ok();
        }

        [EnableQuery]
        [HttpGet("search/odata")]
        [Authorize(Roles = "administrator,moderator,developer,member")]
        public IActionResult Search([FromQuery] string? modelName, [FromQuery] string? material)
        {
            var results = _service.Search(modelName, material);
            return Ok(results);
        }

        [EnableQuery]
        [HttpGet("search")]
        [Authorize(Roles = "administrator,moderator,developer,member")]
        public IActionResult SearchGrouped([FromQuery] string? modelName, [FromQuery] string? material)
        {
            var results = _service.Search(modelName, material);
            var grouped = results
                .GroupBy(h => h.Brand.BrandName)
                .Select(g => new
                {
                    Brand = g.Key,
                    Items = g.ToList()
                });

            return Ok(grouped);
        }
    }
}
