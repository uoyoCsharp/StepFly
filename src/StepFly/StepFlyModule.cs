﻿using MiCake;
using MiCake.AutoMapper;
using MiCake.Core.Modularity;
using StepFly.Domain;
using StepFly.Domain.Repos;
using StepFly.EFCore.Repos;

namespace StepFly
{
    [UseAutoMapper]
    public class StepFlyModule : MiCakeModule
    {
        public override void ConfigServices(ModuleConfigServiceContext context)
        {
            context.RegisterRepository<IStepFlyUserRepository, StepFlyUserRepository>();
            context.RegisterRepository<IHomeConfigRepository, HomeConfigRepository>();
            context.RegisterRepository<INoticeRepository, NoticeRepository>();
            context.RegisterRepository<IUserRoleRepository, UserRoleRepository>();
            context.RegisterRepository<IFeedbackRepository, FeedbackRepository>();
            context.RegisterRepository<IStepFlyHistoryRepository, StepFlyHistoryRepository>();
            context.RegisterRepository<IVIPUserRepository, VIPUserRepository>();
        }
    }
}
