using MiCake;
using MiCake.Core.Modularity;
using StepFly.Domain;
using StepFly.EFCore.Repos;

namespace StepFly
{
    public class StepFlyModule : MiCakeModule
    {
        public override void ConfigServices(ModuleConfigServiceContext context)
        {
            context.RegisterRepository<IStepFlyUserRepository, StepFlyUserRepository>();
        }
    }
}
