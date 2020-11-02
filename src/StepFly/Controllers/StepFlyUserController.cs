using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StepFly.Domain;
using StepFly.Domain.Repos;
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
        private readonly IUserRoleRepository _userRoleRepo;
        private readonly IMapper _mapper;
        public StepFlyUserController(IStepFlyUserRepository userRepo, IUserRoleRepository userRoleRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _userRoleRepo = userRoleRepo;
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

        [HttpPost]
        public async Task<bool> PromotedToAdmin(PromotedToAdminDto dto)
        {
            try
            {
                var user = await _userRepo.FindByUserKeyInfoAsync(dto.UserKeyInfo, (StepFlyProviderType)dto.Type);

                if (user == null)
                    return true;

                await _userRoleRepo.AddAsync(UserRole.Create(user.Id, "admin"));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
