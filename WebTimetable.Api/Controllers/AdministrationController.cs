using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using WebTimetable.Application.Services.Commands;

namespace WebTimetable.Api.Controllers
{
    [ApiVersion(1.0)]
    [ApiController]
    public class AdministrationController : ControllerBase
    {
        private readonly UpdateOutagesCommand _command;

        public AdministrationController(UpdateOutagesCommand command)
        {
            _command = command;
        }
        
        [HttpPut(ApiEndpoints.Administration.UpdateOutages)]
        public async Task<IActionResult> UpdateOutages()
        {
            var isSuccessful = await _command.ExecuteAsync();
            return isSuccessful ? Ok() : Problem();
        }
    }
}