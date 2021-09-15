using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("api/v1/platform")]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformRepository _platformRepository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;

        public PlatformController(IPlatformRepository platformRepository, IMapper mapper, 
            ICommandDataClient commandDataClient)
        {
            _platformRepository = platformRepository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            var platforms = _platformRepository.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(Guid id)
        {
            var platform = _platformRepository.GetPlatformById(id);
            if (platform == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<PlatformReadDto>(platform));
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform([FromBody] PlatformCreateDto model)
        {
            var platformToCreate = _mapper.Map<Platform>(model);
            _platformRepository.CreatePlatform(platformToCreate);
            _platformRepository.SaveChanges();
            var platform = _mapper.Map<PlatformReadDto>(platformToCreate);
            try
            {
                await _commandDataClient.SendPlatformToCommand(platform);
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Could not send synchronously: {e.Message}");
            }
            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platform.Id }, platform);
        }
        
    }
}