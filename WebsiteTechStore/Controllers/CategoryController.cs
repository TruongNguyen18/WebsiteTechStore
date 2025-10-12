using Microsoft.AspNetCore.Mvc;

namespace WebsiteTechStore.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
