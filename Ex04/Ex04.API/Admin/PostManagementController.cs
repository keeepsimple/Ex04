using AutoMapper;
using Ex04.API.DTO;
using Ex04.BusinessLayer.IServices;
using Ex04.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Ex04.API.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class PostManagementController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;

        public PostManagementController(IPostService postService, IMapper mapper, ICategoryService categoryService)
        {
            _postService = postService;
            _mapper = mapper;
            _categoryService = categoryService;
        }

        [HttpGet("GetPagination/{pageNum}")]
        public async Task<IActionResult> GetPagination(string? sortOrder, string? searchString, int? pageNum = 1, int pageSize = 2)
        {
            Expression<Func<Post, bool>> filter = null;
            if (!String.IsNullOrEmpty(searchString))
            {
                filter = f => f.Title.Contains(searchString);
            }

            Func<IQueryable<Post>, IOrderedQueryable<Post>> orderBy = null;
            switch (sortOrder)
            {
                case "Title": orderBy = o => o.OrderBy(x => x.Title); break;
                case "title_desc": orderBy = o => o.OrderByDescending(x => x.Title); break;
                case "CreatedAt": orderBy = o => o.OrderBy(x => x.CreatedAt); break;
                default: orderBy = o => o.OrderByDescending(x => x.CreatedAt); break;
            }
            var posts=await _postService.GetAsync(filter, orderBy, pageNum ?? 1, pageSize);
            var count = await _postService.Count(filter, orderBy);
            var pagingModel = new PagingModel<Post> { List = posts, Count = count };
            return Ok(pagingModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostDTO model)
        {
            var cate = await _categoryService.GetByIdAsync(model.CategoryId);
            if (cate == null) return NotFound("Category not found");
            var post = _mapper.Map<Post>(model);
            var result = await _postService.AddAsync(post);
            if (result > 0)
            {
                return Ok("Create success");
            }
            else
            {
                return BadRequest("Created fail");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var post = await _postService.GetByIdAsync(id);
            if (post == null) return NotFound();
            return Ok(post);
        }

        [HttpPut]
        public async Task<IActionResult> Update(PostDTO model)
        {
            var cate = await _categoryService.GetByIdAsync(model.CategoryId);
            if (cate == null) return NotFound("Category not found");
            var post = await _postService.GetByIdAsync(model.Id);
            if (post == null) return NotFound();

            _mapper.Map(model, post);
            var result = await _postService.UpdateAsync(post);
            if (result)
            {
                return Ok("Update success");
            }
            else
            {
                return BadRequest("Updated fail");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _postService.GetByIdAsync(id);
            if (post == null) return NotFound();
            var result = await _postService.DeleteAsync(post);
            if (result)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Delete fail");
            }
        }
    }
}
