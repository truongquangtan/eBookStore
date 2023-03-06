using BookStoreWebApp.Models;
using BookStoreWebApp.Supporters.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreWebApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly eBookStore5Context context;
        private readonly UserManager<User> _userManager;

        public OrderController(eBookStore5Context context, UserManager<User> userManager)
        {
            this.context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = RoleName.USER)]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var orders = await context.Orders.Where(o => o.UserId == user.Id).OrderByDescending(o => o.UpdateAt).ToListAsync();
            return View(orders);
        }

        [HttpGet]
        [Authorize(Roles = RoleName.USER)]
        [Route("Orders/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var order = await context.Orders.Where(o => o.UserId == user.Id && o.Id == id).FirstOrDefaultAsync();
            if(order == null)
            {
                TempData["Message"] = "You cannot access this order";
                TempData["IsSuccess"] = "false";
                return RedirectToAction("Index");
            }
            var orderDetails = await context.OrderDetails.Where(o => o.OrderId == order.Id).Include(o => o.Product).Include(o => o.Product.ProductImages).ToListAsync();
            return View(orderDetails);
        }
    }
}
