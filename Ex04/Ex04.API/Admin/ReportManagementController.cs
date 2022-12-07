using Ex04.BusinessLayer.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Ex04.API.Admin
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class ReportManagementController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ICategoryService _categoryService;

        public ReportManagementController(IPostService postService, ICategoryService categoryService)
        {
            _postService = postService;
            _categoryService = categoryService;
        }

        [HttpGet("TopRatePost")]
        public IActionResult TopRatePost()
        {
            var posts = _postService.TopRatePost();
            return Ok(posts);
        }

        [HttpGet("TopCategories")]
        public IActionResult TopCategories()
        {
            var categories = _categoryService.GetTopCategories();
            var cate = categories.ToList();
            return Ok(cate);
        }
    }
}
