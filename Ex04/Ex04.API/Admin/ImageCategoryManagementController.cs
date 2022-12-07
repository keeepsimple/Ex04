using AutoMapper;
using Ex04.API.DTO;
using Ex04.BusinessLayer.IServices;
using Ex04.BusinessLayer.Services;
using Ex04.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Ex04.API.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ImageCategoryManagementController : ControllerBase
    {
        private readonly IImageCategoryService _imageCategoryService;
        private readonly IMapper _mapper;

        public ImageCategoryManagementController(IImageCategoryService imageCategoryService, IMapper mapper)
        {
            _imageCategoryService = imageCategoryService;
            _mapper = mapper;
        }

        [HttpGet("GetPagination/{pageNum}")]
        public async Task<IActionResult> GetPagination(string? sortOrder, string? searchString, int? pageNum = 1, int pageSize = 2)
        {
            Expression<Func<ImageCategory, bool>> filter = null;
            if (!String.IsNullOrEmpty(searchString))
            {
                filter = f => f.Name.Contains(searchString);
            }

            Func<IQueryable<ImageCategory>, IOrderedQueryable<ImageCategory>> orderBy = null;
            switch (sortOrder)
            {
                case "Name": orderBy = o => o.OrderBy(x => x.Name); break;
                case "name_desc": orderBy = o => o.OrderByDescending(x => x.Name); break;
                case "CreatedAt": orderBy = o => o.OrderBy(x => x.CreatedAt); break;
                default: orderBy = o => o.OrderByDescending(x => x.CreatedAt); break;
            }
            var imageCategories = await _imageCategoryService.GetAsync(filter, orderBy, pageNum ?? 1, pageSize);
            var count = await _imageCategoryService.Count(filter, orderBy);
            var pagingModel = new PagingModel<ImageCategory> { List = imageCategories, Count = count };
            return Ok(pagingModel);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _imageCategoryService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ImageCategoryDTO model)
        {
            if (!ModelState.IsValid) return BadRequest(model);
            var imageCate = _mapper.Map<ImageCategory>(model);
            var result = await _imageCategoryService.AddAsync(imageCate);
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
            var imageCate = await _imageCategoryService.GetByIdAsync(id);
            if (imageCate == null) return NotFound();
            return Ok(imageCate);
        }

        [HttpPut]
        public async Task<IActionResult> Update(ImageCategoryDTO model)
        {
            var imageCate = await _imageCategoryService.GetByIdAsync(model.Id);
            if (imageCate == null) return NotFound();

            _mapper.Map(model, imageCate);
            var result = await _imageCategoryService.UpdateAsync(imageCate);
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
            var imageCate = await _imageCategoryService.GetByIdAsync(id);
            if (imageCate == null) return NotFound();
            var result = await _imageCategoryService.DeleteAsync(imageCate);
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
