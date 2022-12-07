using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Detail()
        {
            return View();
        }
    }
}
