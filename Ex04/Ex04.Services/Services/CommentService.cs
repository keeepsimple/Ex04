using Ex04.BusinessLayer.BaseServices;
using Ex04.BusinessLayer.IServices;
using Ex04.Data.Infrastructure;
using Ex04.Models;
using Microsoft.EntityFrameworkCore;

namespace Ex04.BusinessLayer.Services
{
    public class CommentService : BaseService<Comment>, ICommentService
    {
        public CommentService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId, bool canLoadDeleted = false)
        {
            return await _unitOfWork.CommentRepository.GetQuery(x => x.PostId == postId && x.IsDeleted == canLoadDeleted).OrderByDescending(x=>x.CreatedAt).ToListAsync();
        }
    }
}
