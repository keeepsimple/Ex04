using Ex04.API.DTO;
using Ex04.BusinessLayer.IServices;
using Ex04.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Ex04.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ICategoryService _categoryService;

        public HomeController(IPostService postService, ICategoryService categoryService)
        {
            _postService = postService;
            _categoryService = categoryService;
        }

        [HttpGet("GetPagination/{pageNum}")]
        public async Task<IActionResult> GetPagination(int? pageNum = 1, int pageSize = 3)
        {
            Func<IQueryable<Post>, IOrderedQueryable<Post>> orderBy = o => o.OrderByDescending(x=>x.CreatedAt);
            var posts = await _postService.GetAsync(null, orderBy, pageNum ?? 1, pageSize);
            var count = await _postService.Count(null, orderBy);
            var pagingModel = new PagingModel<Post> { List = posts, Count = count };
            return Ok(pagingModel);
        }
    }
}
