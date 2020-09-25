using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StepFly.Domain;
using StepFly.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StepFly.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize(Policy = "Admin")]
    public class StepFlyUserController : ApplicationController
    {
        private readonly IStepFlyUserRepository _userRepo;
        private readonly IMapper _mapper;
        public StepFlyUserController(IStepFlyUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<List<StepFlyUserDto>> GetList(int pageIndex, int pageNum)
        {
            return _mapper.Map<List<StepFlyUserDto>>(await _userRepo.GetUserList(pageIndex, pageNum, AbortToken));
        }

        [HttpGet]
        public long GetCount()
        {
            return _userRepo.GetCount();
        }
    }
}
