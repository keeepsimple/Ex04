using AutoMapper;
using Ex04.API.DTO;
using Ex04.BusinessLayer.IServices;
using Ex04.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ex04.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly IRateService _rateService;
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public RateController(IRateService rateService, IPostService postService, IMapper mapper)
        {
            _rateService = rateService;
            _postService = postService;
            _mapper = mapper;
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> RatePost(RateDTO model)
        {
            var rate = _mapper.Map<Rate>(model);
            await _rateService.CreateOrUpdateRate(rate);

            await GetRateByPost(model.PostId);
            return Ok(rate);
        }

        [HttpGet("{postId}/{userId}"), Authorize]
        public IActionResult GetRateInPostByUserId(int postId,string userId)
        {
            var rate = _rateService.GetRateByPostAndUser(postId, userId);
            return Ok(rate);
        }

        private async Task GetRateByPost(int postId)
        {
            var totalRate = _rateService.GetTotalRateInPost(postId);
            var post = await _postService.GetByIdAsync(postId);

            if(post.RateCount > 0)
            {
                post.Rate = totalRate/(decimal)post.RateCount;
                _postService.Update(post);
            }
        }
    }
}
