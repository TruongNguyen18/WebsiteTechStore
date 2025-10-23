using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteTechStore.Models;
using WebsiteTechStore.Repository;

namespace WebsiteTechStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly DataContext _dataContext;
        public OrderController(DataContext context)
        {
            _dataContext = context;
        }
        public async Task<IActionResult> Index(int pg = 1)
        {
            List<OrderModel> order = _dataContext.Orders.ToList();
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = order.Count();
            var pager = new Paginate(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = order.Skip(recSkip).Take(pager.PageSize).ToList();
            ViewBag.Pager = pager;
            return View(data);
        }
        public async Task<IActionResult> ViewOrder(string ordercode)
        {
            var DetailsOrder = await _dataContext.OrderDetails.Include(od => od.Product).Where(od => od.OrderCode==ordercode).ToListAsync(); 
            return View(DetailsOrder);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string ordercode)
        {
            if (string.IsNullOrEmpty(ordercode))
            {
                TempData["error"] = "Mã đơn hàng không hợp lệ!";
                return RedirectToAction("Index");
            }

            // Tìm đơn hàng theo mã
            var order = await _dataContext.Orders
                .FirstOrDefaultAsync(o => o.OrderCode == ordercode);

            if (order == null)
            {
                TempData["error"] = "Không tìm thấy đơn hàng để xóa!";
                return RedirectToAction("Index");
            }

            // Xóa các chi tiết đơn hàng liên quan trước
            var orderDetails = _dataContext.OrderDetails
                .Where(od => od.OrderCode == ordercode);
            _dataContext.OrderDetails.RemoveRange(orderDetails);

            // Xóa đơn hàng chính
            _dataContext.Orders.Remove(order);

            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Đơn hàng đã được xóa thành công!";
            return RedirectToAction("Index");
        }

    }
}
