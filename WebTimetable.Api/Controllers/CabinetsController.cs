using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebTimetable.Application.Services.Abstractions;

namespace WebTimetable.Api.Controllers
{
    [ApiVersion(1.0)]
    [ApiController]
    public class CabinetsController : ControllerBase
    {
        private readonly ICabinetsService _cabinetsService;

        public CabinetsController(ICabinetsService cabinetsService)
        {
            _cabinetsService = cabinetsService;
        }
        
        /// <summary>
        /// Authorized only. Answers whether the cabinet exists in the service.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        [HttpHead(ApiEndpoints.Cabinets.IsCabinetExists)]
        public async Task<IActionResult> IsCabinetExists([FromRoute] string cabinet, CancellationToken token)
        {
            return await _cabinetsService.IsCabinetExistsAsync(cabinet, token) ? Ok() : NotFound();
        }
        
        /// <summary>
        /// Authorized only. Returns path to the cabinet as image.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        [HttpGet(ApiEndpoints.Cabinets.GetCabinetPath)]
        public async Task<IActionResult> GetCabinetPath([FromRoute] string cabinet, CancellationToken token)
        {
            var cabinetPath = await _cabinetsService.GetCabinetImageAsync(cabinet, token);
            
            if (cabinetPath is null)
            {
                return NotFound();
            }
            return File(cabinetPath, "image/jpeg");
        }
    }
}