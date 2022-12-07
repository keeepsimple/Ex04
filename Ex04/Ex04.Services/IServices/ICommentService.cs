using Ex04.BusinessLayer.BaseServices;
using Ex04.Models;

namespace Ex04.BusinessLayer.IServices
{
    public interface ICommentService : IBaseService<Comment>
    {
        Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId, bool canLoadDeleted = false);
    }
}
