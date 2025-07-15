using BLL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/handbags")]
    [ApiController]
    public class HandbagsController : ControllerBase
    {
        private readonly IHandbagService _service;

        public HandbagsController(IHandbagService service)
        {
            _service = service;
        }

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

        [HttpGet("search")]
        [Authorize(Roles = "administrator,moderator,developer,member")]
        public async Task<IActionResult> Search([FromQuery] string modelName, [FromQuery] string material)
        {
            var results = await _service.SearchAsync(modelName, material);
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
