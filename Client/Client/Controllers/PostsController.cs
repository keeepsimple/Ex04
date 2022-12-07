using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class PostsController : Controller
    {
        public IActionResult Detail()
        {
            return View();
        }
    }
}
