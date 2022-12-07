using Ex04.BusinessLayer.BaseServices;
using Ex04.BusinessLayer.IServices;
using Ex04.Data.Infrastructure;
using Ex04.Models;

namespace Ex04.BusinessLayer.Services
{
    public class ImageCategoryService : BaseService<ImageCategory>, IImageCategoryService
    {
        public ImageCategoryService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
