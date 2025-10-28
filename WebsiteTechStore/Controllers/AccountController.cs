using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteTechStore.Models;
using WebsiteTechStore.Models.ViewModels;
using WebsiteTechStore.Repository;

namespace WebsiteTechStore.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUserModel> _userManage;
        private SignInManager<AppUserModel> _signInManage;
        private readonly DataContext _dataContext;
        public AccountController(UserManager<AppUserModel> userManage, SignInManager<AppUserModel> signInManage, DataContext dataContext)
        { 
            _userManage = userManage;
            _signInManage = signInManage;
            _dataContext = dataContext;
        }

        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManage.PasswordSignInAsync(loginVM.UserName,loginVM.Password,false,false);
                if (result.Succeeded)
                {
                    var user = await _userManage.FindByNameAsync(loginVM.UserName);
                    var roles = await _userManage.GetRolesAsync(user);
                    TempData["success"] = "Đăng nhập thành công!!";
                    if (roles.Contains("Admin"))
                    {
                        return RedirectToAction("Index", "Product", new { area = "Admin" });

                    }

                    return Redirect(loginVM.ReturnUrl ?? "/");
                }
                ModelState.AddModelError("", "Invalid Username and Password");
            }
            return View(loginVM);
        }
        public async Task<IActionResult> History()
        {
            if ((bool)!User.Identity?.IsAuthenticated) { 
            return RedirectToAction("Login", "Account");
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var Orders = await _dataContext.Orders
                .Where(o => o.UserName == userEmail).OrderByDescending(od => od.Id)
                .ToListAsync();
            ViewBag.UserEmail = userEmail;
            return View(Orders);
        }
        public async Task<IActionResult> CancelOrder(string ordercode)
        {
            if ((bool)!User.Identity?.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            try
            {
                var order = await _dataContext.Orders.Where(o => o.OrderCode == ordercode).FirstAsync();
                order.Status = 3;
                _dataContext.Update(order);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex) { 
                return BadRequest("Đã có lỗi trong khi hủy đơn hàng");
            }
            
            return RedirectToAction("History","Account");
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserModel user)
        {
            if (ModelState.IsValid)
            {
                AppUserModel newUser = new AppUserModel { UserName = user.UserName, Email = user.Email };
                IdentityResult result = await _userManage.CreateAsync(newUser,user.Password);
                if (result.Succeeded)
                {
                    TempData["success"] = "Tạo thành công user!!";
                    return Redirect("/account/login");
                }
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }
        public async Task<IActionResult> Logout(string returnUrl = "/")
        {
            await _signInManage.SignOutAsync();
            return Redirect(returnUrl);
        }
    }
}
