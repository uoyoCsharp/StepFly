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
    [Authorize()]
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
        [Authorize(Policy = "Admin")]
        public async Task<List<HistoryDto>> Get(int pageIndex, int pageNum, int type)
        {
            return _mapper.Map<List<HistoryDto>>(await _repo.GetHistories(pageIndex, pageNum, (StepFlyProviderType)type));
        }

        [HttpGet]
        public async Task<List<HistoryDto>> GetMyHistories(int pageIndex, int pageNum, string userKey, int type)
        {
            return _mapper.Map<List<HistoryDto>>(await _repo.GetHistoriesByUser(pageIndex, pageNum, userKey, (StepFlyProviderType)type));
        }

        [HttpGet]
        public async Task<HistoryDto> MyLastHistory(string userKey, int type)
        {
            return _mapper.Map<HistoryDto>(await _repo.GetUserLastRecord(userKey, (StepFlyProviderType)type));
        }
    }
}
