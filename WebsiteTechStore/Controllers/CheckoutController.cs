
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WebsiteTechStore.Models;
using WebsiteTechStore.Models.ViewModels;
using WebsiteTechStore.Repository;

namespace WebsiteTechStore.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly DataContext _dataContext;
        public CheckoutController(DataContext context)
        {
            _dataContext = context;
        }
        [HttpGet]
        public IActionResult Address()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(new CheckoutAddressViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Address(CheckoutAddressViewModel model)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return RedirectToAction("Login", "Account");
            }
            List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            if (!cartItems.Any())
            {
                TempData["error"] = "Giỏ hàng trống!";
                return RedirectToAction("Index", "Cart");
            }
            if (!ModelState.IsValid)
                return View(model);
            var ordercode = Guid.NewGuid().ToString("N").ToUpper();
            decimal total = cartItems.Sum(item => item.Price * item.Quantity);
            using var tx = await _dataContext.Database.BeginTransactionAsync();
            try
            {
                var oder = new OrderModel
                {
                    OrderCode = ordercode,
                    UserName = userEmail,
                    CreateDate = DateTime.Now,
                    Status = 0,
                    Province = model.Province,
                    District = model.District,
                    Ward = model.Ward,
                    AddressLine = model.AddressLine,
                    PaymentMethod = model.PaymentMethod,
                    Total = total
                };
                _dataContext.Add(oder);
                await _dataContext.SaveChangesAsync();

                foreach (var item in cartItems)
                {
                    var detail = new OrderDetail
                    {
                        UserName = userEmail,
                        OrderCode = ordercode,
                        ProductId = item.ProductId,
                        Price = item.Price,
                        Quantity = item.Quantity
                    };
                    _dataContext.Add(detail);
                }
                await _dataContext.SaveChangesAsync();
                await tx.CommitAsync();
                HttpContext.Session.Remove("Cart");
                TempData["success"] = "Đặt hàng thành công!! Mã đơn: " + ordercode;
                return RedirectToAction("History", "Account");
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                TempData["error"] = "Đặt hàng thất bại! Lỗi: " + ex.Message;
                return RedirectToAction("Index", "Cart");
            }
        }
        public async Task<IActionResult> Checkout()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else {             
                var ordercode = Guid.NewGuid().ToString();
                var orderItem = new OrderModel();
                orderItem.OrderCode = ordercode;
                orderItem.UserName = userEmail;
                orderItem.CreateDate = DateTime.Now;
                orderItem.Status = 1;
                _dataContext.Add(orderItem);
                _dataContext.SaveChanges();
          
                List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
                foreach (var item in cartItems)
                {
                    var orderDetail = new OrderDetail();
                    orderDetail.UserName = userEmail;
                    orderDetail.OrderCode = ordercode;
                    orderDetail.ProductId = item.ProductId;
                    orderDetail.Price = item.Price;
                    orderDetail.Quantity = item.Quantity;
                    _dataContext.Add(orderDetail);
                    _dataContext.SaveChanges();
                }
                HttpContext.Session.Remove("Cart");
                TempData["success"] = "Đặt hàng thành công!!";
                return RedirectToAction("History", "Account");
            }
                return View();
        }
    }
}
