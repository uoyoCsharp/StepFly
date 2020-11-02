using MiCake.Core.DependencyInjection;
using StepFly.Core;
using StepFly.Domain;
using System.Threading;

namespace StepFly.Services
{
    public interface IStepFlyServiceFactory : IScopedService
    {
        IStepFlyService CreateService(StepFlyProviderType type, CancellationToken cancellationToken = default);
    }
}
