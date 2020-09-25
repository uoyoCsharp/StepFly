using AutoMapper;
using StepFly.Domain;

namespace StepFly.Dtos.Mapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<StepFlyUser, StepFlyUserDto>();
            CreateMap<HomeConfig, HomeConfigDto>();
            CreateMap<Notice, NoticeDto>();
            CreateMap<StepFlyHistory, HistoryDto>();
            CreateMap<FeedBack, FeedbackDto>();
        }
    }
}
