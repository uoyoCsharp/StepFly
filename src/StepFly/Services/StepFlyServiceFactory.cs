using Microsoft.Extensions.DependencyInjection;
using StepFly.Core;
using StepFly.Domain;
using StepFly.Services.LeXin;
using StepFly.Services.XiaoMi;
using System;
using System.Threading;

namespace StepFly.Services
{
    public class StepFlyServiceFactory : IStepFlyServiceFactory
    {
        private readonly IServiceProvider _currentProvider;

        public StepFlyServiceFactory(IServiceProvider serviceProvider)
        {
            _currentProvider = serviceProvider;
        }

        public IStepFlyService CreateService(StepFlyProviderType type, CancellationToken cancellationToken = default)
        {
            switch (type)
            {
                case StepFlyProviderType.LeXin:
                    return _currentProvider.GetService<ILeXinStepFlyService>();
                case StepFlyProviderType.XiaoMi:
                    return _currentProvider.GetService<IXiaoMiStepFlyService>();
                default:
                    return _currentProvider.GetService<IXiaoMiStepFlyService>();
            }
        }
    }
}
