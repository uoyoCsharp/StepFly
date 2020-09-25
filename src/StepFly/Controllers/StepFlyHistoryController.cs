using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StepFly.Domain.Repos;
using StepFly.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StepFly.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize(Policy = "Admin")]
    public class StepFlyHistoryController : ApplicationController
    {
        private readonly IStepFlyHistoryRepository _repo;
        private readonly IMapper _mapper;

        public StepFlyHistoryController(IStepFlyHistoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<List<HistoryDto>> Get(int pageIndex, int pageNum)
        {
            return _mapper.Map<List<HistoryDto>>(await _repo.GetHistories(pageIndex, pageNum));
        }
    }
}
