using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Identity.Web.Resource;

using WebTimetableApi.DAL.Entities;
using WebTimetableApi.DAL.Repositories;
using WebTimetableApi.DTOs;


namespace WebTimetableApi.Controllers
{
    [Authorize]
    [Route("api/auth")]
    [RequiredScope("access_as_user")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly ILogger<AuthorizationController> _logger;
        private readonly GraphServiceClient _graphClient;
        private readonly IDbRepository _dbRepository;
        private readonly IMapper _mapper;
        public AuthorizationController(ILogger<AuthorizationController> logger, IDbRepository dbRepository,
            GraphServiceClient graphServiceClient, IMapper autoMapper)
        {
            _logger = logger;
            _mapper = autoMapper;
            _dbRepository = dbRepository;
            _graphClient = graphServiceClient;
        }

        [HttpPost]
        public async Task<IActionResult> Authorize()
        {
            User graphUser = await _graphClient.Me.GetAsync();

            var existingUser = _dbRepository.Get<UserEntity>(x => x.Id == graphUser.Id).SingleOrDefault();
            if (existingUser != null)
            {
                _logger.LogInformation("Existing user was found. FullName: " + existingUser.FullName);
                
                return Ok(_mapper.Map<UserDTO>(existingUser));
            }

            var user = new UserEntity
            { 
                Id = graphUser.Id,
                FullName = graphUser.DisplayName,
                StudyGroupId = "",
                OutagesGroup = 0
            };
            await _dbRepository.Add(user);
            await _dbRepository.SaveChangesAsync();

            _logger.LogInformation("New user was created. FullName: " + user.FullName);

            return CreatedAtAction(nameof(Authorize), _mapper.Map<UserDTO>(user));
        }
    }
}
