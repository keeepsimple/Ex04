using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    [Route("Admin/[controller]/[action]")]
    public class ReportManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
