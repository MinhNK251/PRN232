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
        private readonly ICosmeticInformationService _cosmeticInformationService;
        public CosmeticInformationsController(ICosmeticInformationService cosmeticInformationService)
        {
            _cosmeticInformationService = cosmeticInformationService;
        }

        [EnableQuery]
        [Authorize(Policy = "AdminOrStaffOrMember")]
        [HttpGet("/api/CosmeticInformations")]
        public async Task<ActionResult<IEnumerable<CosmeticInformation>>> GetCosmeticInformations()
        {
            try
            {
                var result = await _cosmeticInformationService.GetAllCosmetics();
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
                var result = await _cosmeticInformationService.GetAllCategories();
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
            // Sample Post Request Body

            //{
            //      "cosmeticId": "new cosmetic test",
            //      "cosmeticName": "new cosmetic test",
            //      "skinType": "string",
            //      "expirationDate": "string",
            //      "cosmeticSize": "string",
            //      "dollarPrice": 100,
            //      "categoryId": "CAT0101011"
            //}

            try
            {
                var result = await _cosmeticInformationService.Add(cosmeticInformation);
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
            // Sample Put Request Body

            //{
            //      "cosmeticId": "string",
            //      "cosmeticName": "new update",
            //      "skinType": "string",
            //      "expirationDate": "string",
            //      "cosmeticSize": "string",
            //      "dollarPrice": 250,
            //      "categoryId": "CAT0101015"
            //}

            try
            {
                cosmeticInformation.CosmeticId = id;
                var result = await _cosmeticInformationService.Update(cosmeticInformation);
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
                var result = await _cosmeticInformationService.Delete(id);
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
                var result = await _cosmeticInformationService.GetOne(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
        }
    }
}
