using MiCake.Identity.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StepFly.Core;
using StepFly.Domain;
using StepFly.Domain.Repos;
using StepFly.Dtos;
using StepFly.Services;
using StepFly.Services.LeXin;
using StepFly.Services.XiaoMi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StepFly.Controllers
{
    [Route("[controller]/[action]")]
    public class StepFlyController : ApplicationController
    {
        private readonly IStepFlyServiceFactory _stepFlyServiceFactory;
        private readonly IStepFlyUserRepository _userRepo;
        private readonly IJwtSupporter _jwtSupporter;
        private readonly IUserRoleRepository _roleRepository;
        private readonly IStepFlyHistoryRepository _historyRepo;

        public StepFlyController(
            IStepFlyServiceFactory stepFlyServiceFactory,
            IStepFlyUserRepository userRepository,
            IUserRoleRepository roleRepository,
            IStepFlyHistoryRepository historyRepo,
            IJwtSupporter jwtSupporter)
        {
            _stepFlyServiceFactory = stepFlyServiceFactory;
            _userRepo = userRepository;
            _roleRepository = roleRepository;
            _historyRepo = historyRepo;
            _jwtSupporter = jwtSupporter;
        }

        #region 乐心

        [HttpPost]
        public async Task<LoginResultDto> LoginToLeXinByCode(LeXinLoginWithAuthCodeDto loginInfo)
        {
            var leXinLoginModel = new LeXinLoginModel()
            {
                Type = LeXinLoginType.AuthCode,
                UserKeyInfo = loginInfo.Phone,
                AuthCodeLoginInfo = LeXinAuthCodeLoginModel.Create(loginInfo.Phone, loginInfo.Code)
            };

            var service = _stepFlyServiceFactory.CreateService(StepFlyProviderType.LeXin);
            var result = await service.LoginToSystem(leXinLoginModel, AbortToken);

            if (!result.Succeeded)
            {
                return new LoginResultDto()
                {
                    Success = false,
                    Msg = $"{result.Information?.Description} & {result.Information?.PlayLoad}"
                };
            }

            var successModel = result.Information?.PlayLoad as LeXinLoginAPISuccessModel ?? throw new NullReferenceException("没有获取到对应的登录用户");
            var user = successModel.User;
            var userRoles = await _roleRepository.GetUserRoles(user.Id);
            var roleStr = string.Join(",", userRoles.Select(s => s.RoleName).ToArray());

            return new LoginResultDto()
            {
                Success = true,
                Msg = "登录成功",
                UserId = user.Id,
                Token = CreateToken(loginInfo.Phone, user.IsLockout, string.Join(",", userRoles.Select(s => s.RoleName).ToArray())),
                IsLockout = user.IsLockout,
                Roles = roleStr
            };
        }

        [HttpPost]
        public async Task<LoginResultDto> LoginToLeXinByPassword(LeXinLoginWithPasswordDto loginInfo)
        {
            var leXinLoginModel = new LeXinLoginModel()
            {
                Type = LeXinLoginType.Password,
                UserKeyInfo = loginInfo.Phone,
                PasswordLoginInfo = LeXinPasswordLoginModel.Create(loginInfo.Phone, loginInfo.Password)
            };

            var service = _stepFlyServiceFactory.CreateService(StepFlyProviderType.LeXin);
            var result = await service.LoginToSystem(leXinLoginModel, AbortToken);

            if (!result.Succeeded)
            {
                return new LoginResultDto()
                {
                    Success = false,
                    Msg = $"{result.Information?.Description} & {result.Information?.PlayLoad}"
                };
            }
            var successModel = result.Information?.PlayLoad as LeXinLoginAPISuccessModel ?? throw new NullReferenceException("没有获取到对应的登录用户");
            var user = successModel.User;
            var userRoles = await _roleRepository.GetUserRoles(user.Id);
            var roleStr = string.Join(",", userRoles.Select(s => s.RoleName).ToArray());

            return new LoginResultDto()
            {
                Success = true,
                Msg = "登录成功",
                UserId = user.Id,
                Token = CreateToken(loginInfo.Phone, user.IsLockout, roleStr),
                IsLockout = user.IsLockout,
                Roles = roleStr
            };
        }

        [HttpPost]
        [Authorize]
        public async Task<ChangeLeXinStepResultDto> ChangeStepByLeXin(ChangeLeXinStepDto changeDto)
        {
            var service = _stepFlyServiceFactory.CreateService(StepFlyProviderType.LeXin);
            var result = await service.UpdateStepAsync(changeDto.Step, UpdateStepUser.Create(changeDto.Phone), AbortToken);
            if (!result.Succeeded)
            {
                return new ChangeLeXinStepResultDto()
                {
                    Success = false,
                    Msg = result.Information?.Description + " & " + result.Information?.PlayLoad?.ToString(),
                    Code = result.Information?.Code
                };
            }

            await AddChangeHistory(changeDto.Phone, changeDto.Step, StepFlyProviderType.LeXin);

            return new ChangeLeXinStepResultDto()
            {
                Success = true,
                Msg = "修改成功"
            };
        }

        [HttpGet]
        [Authorize]
        public async Task<LeXinBindingType> GetBindingType(string userKey)
        {
            var service = _stepFlyServiceFactory.CreateService(StepFlyProviderType.LeXin);
            var result = await service.GetBindingType(userKey, AbortToken);
            if (!result.Succeeded)
            {
                throw result.Exception;
            }

            return result.Information?.PlayLoad as LeXinBindingType;
        }
        #endregion

        #region 小米

        [HttpPost]
        public async Task<LoginResultDto> LoginToXiaoMiByPassword(XiaoMiLoginWithPasswordDto loginInfo)
        {
            var loginModel = new XiaoMiLoginModel()
            {
                UserPhone = loginInfo.Phone,
                Password = loginInfo.Password
            };

            var service = _stepFlyServiceFactory.CreateService(StepFlyProviderType.XiaoMi);
            var result = await service.LoginToSystem(loginModel, AbortToken);

            if (!result.Succeeded)
            {
                return new LoginResultDto()
                {
                    Success = false,
                    Msg = $"{result.Information?.Description} & {result.Information?.PlayLoad}"
                };
            }
            var successModel = result.Information?.PlayLoad as XiaoMiLoginAPISuccessModel ?? throw new NullReferenceException("没有获取到对应的登录用户");
            var user = successModel.User;
            var userRoles = await _roleRepository.GetUserRoles(user.Id);
            var roleStr = string.Join(",", userRoles.Select(s => s.RoleName).ToArray());

            return new LoginResultDto()
            {
                Success = true,
                Msg = "登录成功",
                UserId = user.Id,
                Token = CreateToken(loginInfo.Phone, user.IsLockout, roleStr),
                IsLockout = user.IsLockout,
                Roles = roleStr
            };
        }

        [HttpPost]
        [Authorize]
        public async Task<ChangeXiaoMiStepResultDto> ChangeStepByXiaoMi(ChangeXiaoMiStepDto changeDto)
        {
            var service = _stepFlyServiceFactory.CreateService(StepFlyProviderType.XiaoMi);
            var result = await service.UpdateStepAsync(changeDto.Step, UpdateStepUser.Create(changeDto.Phone), AbortToken);
            if (!result.Succeeded)
            {
                return new ChangeXiaoMiStepResultDto()
                {
                    Success = false,
                    Msg = result.Information?.Description + " & " + result.Information?.PlayLoad?.ToString(),
                    Code = result.Information?.Code
                };
            }

            await AddChangeHistory(changeDto.Phone, changeDto.Step, StepFlyProviderType.XiaoMi);

            return new ChangeXiaoMiStepResultDto()
            {
                Success = true,
                Msg = "修改成功"
            };
        }

        #endregion

        private async Task AddChangeHistory(string userKey, int num, StepFlyProviderType type)
        {
            var record = new StepFlyHistory(userKey, num);
            record.SetSource(type);

            await _historyRepo.AddAsync(record, AbortToken);
        }

        private string CreateToken(string userKey, bool userLock, string userRoles)
        {
            var claims = new List<Claim>()
            {
                new Claim("sub",userKey),
                new Claim("lockout",userLock ? "1":"0"),
            };

            if (!string.IsNullOrWhiteSpace(userRoles))
                claims.Add(new Claim(ClaimTypes.Role, userRoles));

            return _jwtSupporter.CreateToken(new ClaimsIdentity(claims));
        }
    }
}
