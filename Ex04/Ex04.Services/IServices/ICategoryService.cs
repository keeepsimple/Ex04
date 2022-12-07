using Ex04.BusinessLayer.BaseServices;
using Ex04.Models;

namespace Ex04.BusinessLayer.IServices
{
    public interface ICategoryService : IBaseService<Category>
    {
        Dictionary<Category, int> GetTopCategories();
    }
}
