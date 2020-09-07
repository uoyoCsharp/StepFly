using MiCake.Identity.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StepFly.Core;
using StepFly.Domain;
using StepFly.Dtos;
using StepFly.Services.LeXin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StepFly.Controllers
{
    [Route("[controller]/[action]")]
    public class StepFlyController : ApplicationController
    {
        private readonly IStepFlyService _stepFlyService;
        private readonly IStepFlyUserRepository _userRepo;
        private readonly IJwtSupporter _jwtSupporter;

        public StepFlyController(
            IStepFlyService stepFlyService,
            IStepFlyUserRepository userRepository,
            IJwtSupporter jwtSupporter)
        {
            _stepFlyService = stepFlyService;
            _userRepo = userRepository;
            _jwtSupporter = jwtSupporter;
        }

        [HttpPost]
        public async Task<LoginResultDto> LoginToLeXinByCode(LeXinLoginWithAuthCodeDto loginInfo)
        {
            var leXinLoginModel = new LeXinLoginModel()
            {
                Type = LeXinLoginType.AuthCode,
                UserKeyInfo = loginInfo.Phone,
                AuthCodeLoginInfo = LeXinAuthCodeLoginModel.Create(loginInfo.Phone, loginInfo.Code)
            };

            var result = await _stepFlyService.LoginToSystem(leXinLoginModel, AbortToken);

            if (!result.Succeeded)
            {
                return new LoginResultDto()
                {
                    Success = false,
                    Msg = $"{result.Information?.Description} & {result.Information?.PlayLoad}"
                };
            }

            return new LoginResultDto()
            {
                Success = true,
                Msg = "登录成功",
                Token = CreateToken(loginInfo.Phone)
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

            var result = await _stepFlyService.LoginToSystem(leXinLoginModel, AbortToken);

            if (!result.Succeeded)
            {
                return new LoginResultDto()
                {
                    Success = false,
                    Msg = $"{result.Information?.Description} & {result.Information?.PlayLoad}"
                };
            }

            return new LoginResultDto()
            {
                Success = true,
                Msg = "登录成功",
                Token = CreateToken(loginInfo.Phone)
            };
        }

        [HttpPost]
        [Authorize]
        public async Task<ChangeLeXinStepResultDto> ChangeStepByLeXin(ChangeLeXinStepDto changeDto)
        {
            var result = await _stepFlyService.UpdateStepAsync(changeDto.Step, UpdateStepUser.Create(changeDto.Phone), AbortToken);
            if (!result.Succeeded)
            {
                return new ChangeLeXinStepResultDto()
                {
                    Success = false,
                    Msg = result.Information?.Description + " & " + result.Information?.PlayLoad.ToString(),
                    Code = result.Information?.Code
                };
            }

            return new ChangeLeXinStepResultDto()
            {
                Success = true,
                Msg = "修改成功"
            };
        }

        [HttpGet]
        public async Task<bool> HasLoginInfoWithLeXin([Phone]string phone)
        {
            var user = await _userRepo.FindByUserKeyInfoAsync(phone);

            if (user == null)
                return false;

            if (user!.TokenExpireTime <= DateTime.Now)
                return false;

            return true;
        }

        private string CreateToken(string userKey)
        {
            return _jwtSupporter.CreateToken(new ClaimsIdentity(new List<Claim>()
            {
                new Claim("sub",userKey)
            }));
        }
    }
}
