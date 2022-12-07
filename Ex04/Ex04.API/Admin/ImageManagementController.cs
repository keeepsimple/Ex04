using AutoMapper;
using Ex04.API.DTO;
using Ex04.BusinessLayer.IServices;
using Ex04.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Ex04.API.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ImageManagementController : ControllerBase
    {
        private readonly IImageService _imageService;
        private readonly IImageCategoryService _imageCategoryService;
        private readonly IImageAndCategoryService _imageAndCategoryService;
        private readonly IMapper _mapper;
        private IWebHostEnvironment _webHost;

        public ImageManagementController(IImageService imageService,
            IImageCategoryService imageCategoryService,
            IImageAndCategoryService imageAndCategoryService,
            IMapper mapper,
            IWebHostEnvironment webHost)
        {
            _imageService = imageService;
            _imageCategoryService = imageCategoryService;
            _imageAndCategoryService = imageAndCategoryService;
            _mapper = mapper;
            _webHost = webHost;
        }

        [HttpGet("GetPagination/{pageNum}")]
        public async Task<IActionResult> GetPagination(string? sortOrder, int? imageCategory, string? searchString, int? pageNum = 1, int pageSize = 2)
        {
            Expression<Func<Image, bool>> filter = null;

            if (!String.IsNullOrEmpty(searchString))
            {
                filter = f => f.Name.Contains(searchString);
            }

            if (imageCategory != null)
            {
                filter = f => f.ImageAndCategories.Any(x => x.ImageCategoryId == imageCategory);
            }

            if (imageCategory != null && !String.IsNullOrEmpty(searchString))
            {
                filter = f => f.ImageAndCategories.Any(x => x.ImageCategoryId == imageCategory)
                             && f.Name.Contains(searchString);
            }

            Func<IQueryable<Image>, IOrderedQueryable<Image>> orderBy = null;
            switch (sortOrder)
            {
                case "Name": orderBy = o => o.OrderBy(x => x.Name); break;
                case "name_desc": orderBy = o => o.OrderByDescending(x => x.Name); break;
                case "createdAt_asc": orderBy = o => o.OrderBy(x => x.CreatedAt); break;
                default: orderBy = o => o.OrderByDescending(x => x.CreatedAt); break;
            }

            var images = await _imageService.GetAsync(filter, orderBy, pageNum ?? 1, pageSize);
            var count = await _imageService.Count(filter, null);
            var pagingModel = new PagingModel<Image> { List = images, Count = count };
            return Ok(pagingModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ImageDTO model)
        {
            if (!ModelState.IsValid) return BadRequest(model);
            string folderPath = _webHost.WebRootPath + "\\images\\";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var file = Path.Combine(_webHost.WebRootPath, folderPath, model.UploadImage.FileName);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await model.UploadImage.CopyToAsync(fileStream);
            }

            var image = _mapper.Map<Image>(model);
            image.ImageUrl = model.UploadImage.FileName;
            image.Size = model.Size / 1024;
            var result = await _imageService.AddAsync(image);
            if (result > 0)
            {
                await GetSelectedIds(model.ImageCateIds, image);
                return Ok("Create success");
            }
            else
            {
                return BadRequest("Created fail");
            }
        }

        private async Task GetSelectedIds(IEnumerable<int> ids, Image image)
        {
            foreach (var id in ids)
            {
                var imageCategory = await _imageCategoryService.GetByIdAsync(id);
                if (imageCategory != null)
                {
                    var imageAndCate = new @int
                    {
                        ImageCategoryId = imageCategory.Id,
                        ImageId = image.Id
                    };
                    await _imageAndCategoryService.AddAsync(imageAndCate);
                }
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var image = await _imageService.GetByIdAsync(id);
            if (image == null) return NotFound();
            return Ok(image);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var image = await _imageService.GetByIdAsync(id);
            if (image == null) return NotFound();
            var result = await _imageService.DeleteAsync(image);
            string folderPath = _webHost.WebRootPath + "\\images\\" + image.ImageUrl;
            System.IO.File.Delete(folderPath);
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
