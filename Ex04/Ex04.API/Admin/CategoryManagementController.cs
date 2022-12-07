using AutoMapper;
using Ex04.API.DTO;
using Ex04.BusinessLayer.IServices;
using Ex04.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq.Expressions;

namespace Ex04.API.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryManagementController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryManagementController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet("GetPagination/{pageNum}")]
        public async Task<IActionResult> GetPagination(string? sortOrder, string? searchString, int? pageNum = 1, int pageSize = 4)
        {
            Expression<Func<Category, bool>> filter = null;
            if (!String.IsNullOrEmpty(searchString))
            {
                filter = f => f.Title.Contains(searchString);
            }

            Func<IQueryable<Category>, IOrderedQueryable<Category>> orderBy = null;
            switch (sortOrder)
            {
                case "Title": orderBy = o => o.OrderBy(x => x.Title); break;
                case "title_desc": orderBy = o => o.OrderByDescending(x => x.Title); break;
                case "CreatedAt": orderBy = o => o.OrderBy(x => x.CreatedAt); break;
                default: orderBy = o => o.OrderByDescending(x => x.CreatedAt); break;
            }

            var categories = await _categoryService.GetAsync(filter, orderBy, pageNum ?? 1, pageSize);
            var count = await _categoryService.Count(filter, orderBy);
            var pagingModel = new PagingModel<Category> { List = categories, Count = count };
            return Ok(pagingModel);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _categoryService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDTO model)
        {
            if (!ModelState.IsValid) return BadRequest(model);
            var cate = _mapper.Map<Category>(model);
            var result = await _categoryService.AddAsync(cate);
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
            var cate = await _categoryService.GetByIdAsync(id);
            if (cate == null) return NotFound();
            return Ok(cate);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CategoryDTO model)
        {
            var cate = await _categoryService.GetByIdAsync(model.Id);
            if (cate == null) return NotFound();

            _mapper.Map(model, cate);
            var result = await _categoryService.UpdateAsync(cate);
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
            var cate = await _categoryService.GetByIdAsync(id);
            if (cate == null) return NotFound();
            var result = await _categoryService.DeleteAsync(cate);
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

