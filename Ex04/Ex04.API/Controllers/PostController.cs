using Ex04.BusinessLayer.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ex04.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> Get(int id) 
        { 
            var post = await _postService.GetByIdAsync(id);
            post.Views++;
            await _postService.UpdateAsync(post);
            return Ok(post);
        }

        [HttpGet("Other")]
        public async Task<IActionResult> OtherPostInCate(int postId, int cateId)
        {
            var posts = await _postService.GetPostsByCateId(cateId);
            posts = posts.Take(5).Except(posts.Where(x => x.Id == postId)).OrderByDescending(x => x.CreatedAt).ToList();
            return Ok(posts);
        }
    }
}
