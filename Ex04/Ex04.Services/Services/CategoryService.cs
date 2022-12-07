using Ex04.BusinessLayer.BaseServices;
using Ex04.BusinessLayer.IServices;
using Ex04.Data.Infrastructure;
using Ex04.Models;

namespace Ex04.BusinessLayer.Services
{
    public class CategoryService : BaseService<Category>, ICategoryService
    {
        public CategoryService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public Dictionary<Category, int> GetTopCategories()
        {
            var viewCategories = from p in _unitOfWork.PostRepository.GetQuery(x=>x.IsDeleted == false) select new { p.CategoryId, p.Views };
            var categories = _unitOfWork.CategoryRepository.GetQuery(x => x.IsDeleted == false).ToList();
            Dictionary<Category, int> topCategory = new Dictionary<Category, int>();
            categories.ForEach(category =>
            {
                var totalView = viewCategories.Where(x => x.CategoryId == category.Id).Sum(x => x.Views);
                topCategory.Add(category, totalView);
            });
            return topCategory.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
