using Ex04.BusinessLayer.BaseServices;
using Ex04.Models;

namespace Ex04.BusinessLayer.IServices
{
    public interface IPostService : IBaseService<Post>
    {
        Task<IEnumerable<Post>> GetPostsByCateId(int cateId);

        IEnumerable<Post> TopRatePost();
    }
}
