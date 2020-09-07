using AutoMapper;
using StepFly.Domain;

namespace StepFly.Dtos.Mapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<HomeConfig, HomeConfigDto>();
            CreateMap<Notice, NoticeDto>();
        }
    }
}
