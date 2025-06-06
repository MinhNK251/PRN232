using BusinessObjects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CosmeticInformationsController : ControllerBase
    {
        private readonly ICosmeticInformationService _cismeticInformationService;
        public CosmeticInformationsController(ICosmeticInformationService cismeticInformationService)
        {
            _cismeticInformationService = cismeticInformationService;
        }

        [EnableQuery]
        [Authorize(Policy = "AdminOrStaffOrMember")]
        [HttpGet("/api/CosmeticInformations")]
        public async Task<ActionResult<IEnumerable<CosmeticInformation>>> GetCosmeticInformations()
        {
            try
            {
                var result = await _cismeticInformationService.GetAllCosmetics();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }

        [Authorize(Policy = "AdminOrStaffOrMember")]
        [HttpGet("/api/CosmeticCategories")]
        public async Task<ActionResult<List<CosmeticCategory>>> GetCategories()
        {
            try
            {
                var result = await _cismeticInformationService.GetAllCategories();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("/api/CosmeticInformations")]
        public async Task<ActionResult<CosmeticInformation>> AddCosmeticInformation([FromBody] CosmeticInformation cosmeticInformation)
        {
            try
            {
                var result = await _cismeticInformationService.Add(cosmeticInformation);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("/api/CosmeticInformations/{id}")]
        public async Task<ActionResult<CosmeticInformation>> UpdateCosmeticInformation(string id, [FromBody] CosmeticInformation cosmeticInformation)
        {
            try
            {
                cosmeticInformation.CosmeticId = id;
                var result = await _cismeticInformationService.Update(cosmeticInformation);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("/api/CosmeticInformations/{id}")]
        public async Task<ActionResult<CosmeticInformation>> DeleteCosmeticInformation(string id)
        {
            try
            {
                var result = await _cismeticInformationService.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }


        [Authorize(Policy = "AdminOrStaffOrMember")]
        [HttpGet("/api/CosmeticInformations/{id}")]
        public async Task<ActionResult<CosmeticInformation>> AddCosmeticInformation(string id)
        {
            try
            {
                var result = await _cismeticInformationService.GetOne(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }
    }
}
