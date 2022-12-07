using Ex04.BusinessLayer.BaseServices;
using Ex04.BusinessLayer.IServices;
using Ex04.Data.Infrastructure;
using Ex04.Models;
using Microsoft.EntityFrameworkCore;

namespace Ex04.BusinessLayer.Services
{
    public class RateService : BaseService<Rate>, IRateService
    {
        public RateService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task CreateOrUpdateRate(Rate entity)
        {
            var rate = await _unitOfWork.RateRepository.GetQuery(x => x.PostId == entity.PostId
                                                                     && x.UserId == entity.UserId).FirstOrDefaultAsync();
            if (rate != null)
            {
                rate.TotalRate = entity.TotalRate;
                _unitOfWork.RateRepository.Update(rate);
                await _unitOfWork.SaveChangesAsync();

            }
            else
            {
                var post = _unitOfWork.PostRepository.GetQuery(x => x.Id == entity.PostId).FirstOrDefault();
                post.RateCount += 1;
                _unitOfWork.PostRepository.Update(post);
                _unitOfWork.RateRepository.Add(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public Rate GetRateByPostAndUser(int postId, string userId)
        {
            var rate = _unitOfWork.RateRepository.GetQuery(x => x.PostId == postId && x.UserId == userId).FirstOrDefault();
            if (rate != null)
            {
                return rate;
            }
            return null;
        }

        public int GetTotalRateInPost(int postId)
        {
            var rates = _unitOfWork.RateRepository.GetQuery(x => x.PostId == postId).ToList();
            return rates.Sum(x => x.TotalRate);
        }
    }
}
