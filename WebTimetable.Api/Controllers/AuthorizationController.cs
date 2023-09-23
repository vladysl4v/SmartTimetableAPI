using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;

using WebTimetable.Api;
using WebTimetable.Api.Mapping;
using WebTimetable.Application.Entities;
using WebTimetable.Application.Repositories;


namespace WebTimetable.Api.Controllers
{
    [Authorize]
    [RequiredScope("access_as_user")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly ILogger<AuthorizationController> _logger;
        private readonly IDbRepository _dbRepository;
        public AuthorizationController(ILogger<AuthorizationController> logger, IDbRepository dbRepository)
        {
            _logger = logger;
            _dbRepository = dbRepository;
        }

        [HttpPost(ApiEndpoints.Authorization.Authorize)]
        public async Task<IActionResult> Authorize()
        {
            var graphId = User.GetObjectId();

            var existingUser = _dbRepository.Get<UserEntity>(x => x.Id == User.GetObjectId()).SingleOrDefault();
            if (existingUser != null)
            {
                _logger.LogInformation("Existing user was found. FullName: " + existingUser.FullName);
                
                return Ok(existingUser.MapToResponse());
            }

            var user = new UserEntity
            { 
                Id = User.GetObjectId(),
                FullName = User.GetDisplayName(),
                StudyGroupId = "",
                OutagesGroup = 0
            };
            await _dbRepository.Add(user);
            await _dbRepository.SaveChangesAsync();

            _logger.LogInformation("New user was created. FullName: " + user.FullName);

            return CreatedAtAction(nameof(Authorize), user.MapToResponse());
        }
    }
}
