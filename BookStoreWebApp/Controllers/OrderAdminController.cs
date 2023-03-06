using BookStoreWebApp.Models;
using BookStoreWebApp.Models.Request;
using BookStoreWebApp.Supporters.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreWebApp.Controllers
{
    [Authorize(Roles = RoleName.ADMIN)]
    public class OrderAdminController : Controller
    {
        private readonly eBookStore5Context context;
        private readonly UserManager<User> _userManager;

        public OrderAdminController(eBookStore5Context context, UserManager<User> userManager)
        {
            this.context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var orders = await context.Orders.OrderByDescending(o => o.OrderAt).ToListAsync();
            return View(orders);
        }

        [HttpGet]
        [Route("Admin/Orders/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var order = context.Orders.Where(o => o.Id == id).FirstOrDefault();
            if(order == null)
            {
                return NotFound();
            }
            var orderDetails = await context.OrderDetails.Where(o => o.OrderId == id).Include(o => o.Product).Include(o => o.Product.ProductImages).ToListAsync();
            order.OrderDetails = orderDetails;
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(ChangeOrderStatusRequest request)
        {
            if(request.NewStatus != OrderStatus.CREATED
                && request.NewStatus != OrderStatus.ACCEPTED
                && request.NewStatus != OrderStatus.SHIPPED)
            {
                return BadRequest();
            }
            var order = context.Orders.Where(o => o.Id == request.OrderId).FirstOrDefault();
            if(order == null)
            {
                return NotFound();
            }

            order.Status = request.NewStatus;
            context.Orders.Update(order);
            await context.SaveChangesAsync();
            TempData["Message"] = "You've change status of a order successfully.";
            TempData["IsSuccess"] = "true";
            return RedirectToAction("Details", new { id = request.OrderId });
        }
    }
}
