using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using StepFly.Domain;
using StepFly.Domain.Repos;
using StepFly.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<List<StepFlyUserDto>> List(int pageIndex, int pageNum, int type)
        {
            return _mapper.Map<List<StepFlyUserDto>>(await _userRepo.GetUserList(pageIndex, pageNum, (StepFlyProviderType)type, AbortToken));
        }

        [HttpGet]
        public async Task<long> Count(int type)
        {
            return await _userRepo.GetCountByType((StepFlyProviderType)type);
        }

        [HttpGet]
        public async Task<StepFlyUserDto> Detail(Guid id)
        {
            return _mapper.Map<StepFlyUserDto>(await _userRepo.FindAsync(id, AbortToken));
        }

        [HttpPut]
        public async Task<bool> Lock(Guid userId)
        {
            var user = await _userRepo.FindAsync(userId, AbortToken);
            if (user == null)
                return false;

            user.LockUser();
            return true;
        }

        [HttpPost]
        public async Task<bool> PromotedToAdmin(PromotedToAdminDto dto)
        {
            try
            {
                var user = await _userRepo.FindAsync(dto.UserId, AbortToken);

                if (user == null)
                    return true;

                var userCurrentRole = await _userRoleRepo.GetUserRoles(dto.UserId, AbortToken);
                if (!userCurrentRole.Any(s => s.RoleName == "admin"))
                {
                    await _userRoleRepo.AddAsync(UserRole.Create(user.Id, "admin"));
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
