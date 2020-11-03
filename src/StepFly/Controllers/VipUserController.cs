using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StepFly.Domain;
using StepFly.Domain.Repos;
using StepFly.Dtos;
using System;
using System.Threading.Tasks;

namespace StepFly.Controllers
{
    [Route("[controller]/[action]")]
    public class VIPUserController : ApplicationController
    {
        private readonly IStepFlyUserRepository _userRepo;
        private readonly IVIPUserRepository _vipRepo;
        private readonly IMapper _mapper;

        public VIPUserController(IStepFlyUserRepository userRepo, IVIPUserRepository vipRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _vipRepo = vipRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<VIPUserDto> GetVIPInfo(Guid userId)
        {
            var vipUser = await _vipRepo.FindByUserId(userId, AbortToken);
            bool isVip = vipUser != null && vipUser.ExpireTime > DateTime.Now;

            return new VIPUserDto()
            {
                IsVip = isVip,
                ExpireTime = vipUser?.ExpireTime,
                Level = vipUser?.Level ?? 0,
            };
        }
    }
}
