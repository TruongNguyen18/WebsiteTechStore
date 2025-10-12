using Microsoft.AspNetCore.Mvc;

namespace WebsiteTechStore.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
