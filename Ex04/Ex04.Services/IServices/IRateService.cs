using Ex04.BusinessLayer.BaseServices;
using Ex04.Models;

namespace Ex04.BusinessLayer.IServices
{
    public interface IRateService : IBaseService<Rate>
    {
        Task CreateOrUpdateRate(Rate entity);

        Rate GetRateByPostAndUser(int postId, string userId);

        int GetTotalRateInPost(int postId);
    }
}
