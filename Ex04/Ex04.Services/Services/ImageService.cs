using Ex04.BusinessLayer.BaseServices;
using Ex04.BusinessLayer.IServices;
using Ex04.Data.Infrastructure;
using Ex04.Models;

namespace Ex04.BusinessLayer.Services
{
    public class ImageService : BaseService<Image>, IImageService
    {
        public ImageService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
