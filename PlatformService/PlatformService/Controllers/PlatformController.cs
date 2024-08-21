using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using AutoMapper;
using PlatformService.DTOs;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformController : ControllerBase
    {
        private readonly ICommandDataClient _commandDataClient;
        private readonly IPlatformRepo _platformRepo;
        private readonly IMapper _mapper;

        public PlatformController(
            IPlatformRepo platformRepo,
            IMapper mapper,
            ICommandDataClient commandDataClient
            )
        {
            _commandDataClient = commandDataClient;
            _platformRepo = platformRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatForms()
        {
            var platforms = _platformRepo.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }

        [HttpGet("{id}", Name = "GetPlatformById")] 
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platform = _platformRepo.GetPlatformById(id);

            if (platform == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(_mapper.Map<PlatformReadDto>(platform));
            }
        } 
        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {

            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            _platformRepo.CreatePlatform(platformModel);
            _platformRepo.SaveChange();
            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

            try
            {
                await _commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
           
            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformReadDto);
        }

    }

}
