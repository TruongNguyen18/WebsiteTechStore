using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebsiteTechStore.Models;
using WebsiteTechStore.Models.ViewModels;
using WebsiteTechStore.Repository;

namespace WebsiteTechStore.Controllers
{
    public class CartController : Controller
    {
        private readonly DataContext _dataContext;
        public CartController(DataContext _context)
        {
            _dataContext = _context;
        }
        public IActionResult Index()
        {
            List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemViewModel cartVM = new()
            {
                CartItems = cartItems,
                CartTotal = cartItems.Sum(x => x.Quantity * x.Price)
            };
            return View(cartVM);
        }
        public ActionResult Checkout()
        {
            return View();
        }
        public async Task<IActionResult> Add (int Id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(Id);
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemModel cartItems = cart.Where(c => c.ProductId == Id).FirstOrDefault();

            if(cartItems == null)
            {
                cart.Add(new CartItemModel(product));
            }
            else
            {
                cartItems.Quantity += 1;
            }

            HttpContext.Session.SetJson("Cart", cart);
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
