using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StepFly.Domain;
using StepFly.Domain.Repos;
using StepFly.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StepFly.Controllers
{
    [Route("[controller]/[action]")]
    public class FeedbackController : ApplicationController
    {
        private readonly IFeedbackRepository _repo;
        private readonly IMapper _mapper;

        public FeedbackController(IFeedbackRepository repository, IMapper mapper)
        {
            _repo = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<List<FeedbackDto>> Get(int pageIndex, int pageNum)
        {
            return _mapper.Map<List<FeedbackDto>>(await _repo.GetFeedBacks(pageIndex, pageNum, AbortToken));
        }

        [HttpPost]
        [Authorize]
        public async Task<bool> Add(AddFeedbackDto dto)
        {
            var hasRecordOfToday = await _repo.GetUserTodayFeedbacks(dto.UserKey, AbortToken);

            if (hasRecordOfToday.Count > 0)
                return false;

            var feedback = FeedBack.Create(dto.Title, dto.Content);
            feedback.SetFeedBackUser(dto.UserKey);
            feedback.SetTag(dto.Tag);
            await _repo.AddAsync(feedback);

            return true;
        }
    }
}
