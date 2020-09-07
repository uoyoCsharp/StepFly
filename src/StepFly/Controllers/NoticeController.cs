using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StepFly.Domain;
using StepFly.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StepFly.Controllers
{
    [Route("[controller]/[action]")]
    public class NoticeController : ApplicationController
    {
        private readonly IMapper _mapper;
        private readonly INoticeRepository _repo;

        public NoticeController(INoticeRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<NoticeDto>> GetNotices()
           => _mapper.Map<IEnumerable<NoticeDto>>(await _repo.GetValidNotices(AbortToken));
    }
}
