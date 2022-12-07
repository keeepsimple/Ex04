using Ex04.API.DTO;
using Ex04.BusinessLayer.IServices;
using Ex04.BusinessLayer.Services;
using Ex04.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq.Expressions;

namespace Ex04.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IPostService _postService;

        public CategoryController(ICategoryService categoryService, IPostService postService)
        {
            _categoryService = categoryService;
            _postService = postService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) 
        {
            var cate = await _categoryService.GetByIdAsync(id);

            return Ok(cate);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Get(int id, int? pageNum = 1, int pageSize = 5)
        {
            Expression<Func<Post, bool>> filter = f => f.CategoryId == id;
            Func<IQueryable<Post>, IOrderedQueryable<Post>> orderBy = o => o.OrderByDescending(x => x.CreatedAt);
            var posts = await _postService.GetAsync(filter, orderBy, pageNum ?? 1, pageSize);
            var count = await _postService.Count(filter, orderBy);
            var pagingModel = new PagingModel<Post> { List = posts, Count = count };
            return Ok(pagingModel);
        }
    }
}
