﻿using MiCake.Core.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace StepFly.Core
{
    public interface IStepFlyService : IScopedService
    {
        /// <summary>
        /// 登录到需要获取步数的平台系统
        /// </summary>
        /// <typeparam name="T">登录model</typeparam>
        Task<OperateResult> LoginToSystem<T>(T loginIno, CancellationToken cancellationToken = default);

        /// <summary>
        /// 修改为指定的步数
        /// </summary>
        Task<OperateResult> UpdateStepAsync(int stepNum, UpdateStepUser userInfo, CancellationToken cancellationToken = default);
    }
}
