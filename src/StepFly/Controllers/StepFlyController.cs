using MiCake.Core;
using Microsoft.AspNetCore.Mvc;
using StepFly.Core;
using StepFly.Domain;
using StepFly.Dtos;
using StepFly.Services.LeXin;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace StepFly.Controllers
{
    [Route("[controller]/[action]")]
    public class StepFlyController : ApplicationController
    {
        private readonly IStepFlyService _stepFlyService;
        private readonly IStepFlyUserRepository _userRepo;

        public StepFlyController(IStepFlyService stepFlyService, IStepFlyUserRepository userRepository)
        {
            _stepFlyService = stepFlyService;
            _userRepo = userRepository;
        }

        [HttpPost]
        public async Task<bool> LoginToLeXinByCode(LeXinLoginWithAuthCodeDto loginInfo)
        {
            var leXinLoginModel = new LeXinLoginModel()
            {
                Type = LeXinLoginType.AuthCode,
                AuthCodeLoginInfo = LeXinAuthCodeLoginModel.Create(loginInfo.Phone, loginInfo.Code)
            };

            var result = await _stepFlyService.LoginToSystem(leXinLoginModel, AbortToken);

            if (!result.Succeeded)
            {
                throw new SoftlyMiCakeException(result.Information?.Description);
            }

            return true;
        }

        [HttpPost]
        public async Task<bool> ChangeStepByLeXin(ChangeLeXinStepDto changeDto)
        {
            var result = await _stepFlyService.UpdateStepAsync(changeDto.Step, UpdateStepUser.Create(changeDto.Phone), AbortToken);
            if (!result.Succeeded)
            {
                throw new SoftlyMiCakeException(result.Information?.Description + " & " + result.Information?.PlayLoad.ToString());
            }

            return true;
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
    }
}
