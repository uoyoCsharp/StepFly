using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StepFly.Domain;
using StepFly.Domain.Repos;
using StepFly.Dtos;
using System;
using System.Threading.Tasks;

namespace StepFly.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize(Policy = "Admin")]
    public class VIPUserManageController : ApplicationController
    {
        private readonly IStepFlyUserRepository _userRepo;
        private readonly IVIPUserRepository _vipRepo;
        private readonly IMapper _mapper;

        public VIPUserManageController(IStepFlyUserRepository userRepo, IVIPUserRepository vipRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _vipRepo = vipRepo;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<bool> ActiveVIP(ActiveVIPDto dto)
        {
            var vipRecord = await _vipRepo.FindByUserId(dto.UserId, AbortToken);
            if (vipRecord == null)
            {
                await _vipRepo.AddAsync(VIPUser.Create(dto.UserId, 365), AbortToken);
                return true;
            }

            return false;
        }

        [HttpPost]
        public async Task<bool> StopVIP(StopVIPDto dto)
        {
            var vipRecord = await _vipRepo.FindByUserId(dto.UserId, AbortToken);
            if (vipRecord != null)
            {
                vipRecord.ExpireTime = DateTime.Now;
                return true;
            }
            return false;
        }
    }
}
