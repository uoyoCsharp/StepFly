using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StepFly.Domain;
using StepFly.Dtos;
using System;
using System.Threading.Tasks;

namespace StepFly.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : ApplicationController
    {
        private readonly IMapper _mapper;
        private readonly IHomeConfigRepository _repo;

        public HomeController(IHomeConfigRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<HomeConfigDto> GetHomeConfig()
        {
            return _mapper.Map<HomeConfigDto>(await _repo.GetConfig());
        }

        [HttpGet]
        public VersionDto Version()
        {
            return new VersionDto()
            {
                Version = new Version(1, 0, 3).ToString(),
                CompatibleVersion = new Version(1, 0, 3).ToString()
            };
        }
    }
}
