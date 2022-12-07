using Ex04.BusinessLayer.BaseServices;
using Ex04.BusinessLayer.IServices;
using Ex04.Data.Infrastructure;
using Ex04.Models;
using Microsoft.EntityFrameworkCore;

namespace Ex04.BusinessLayer.Services
{
    public class PostService : BaseService<Post>, IPostService
    {
        public PostService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<IEnumerable<Post>> GetPostsByCateId(int cateId)
        {
            return await _unitOfWork.PostRepository.GetQuery(x => x.CategoryId == cateId).ToListAsync();
        }

        public IEnumerable<Post> TopRatePost()
        {
            return _unitOfWork.PostRepository.GetQuery(x => x.IsDeleted == false).OrderByDescending(x => x.Rate).Take(10).ToList();
        }
    }
}
