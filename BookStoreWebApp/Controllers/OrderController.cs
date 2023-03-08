using BookStoreWebApp.Models;
using BookStoreWebApp.Supporters.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.OrderDetailRepository;
using Repositories.Repositories.OrderRepository;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreWebApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository orderRepository;
        private readonly IOrderDetailRepository orderDetailRepository;
        private readonly UserManager<User> _userManager;

        public OrderController(UserManager<User> userManager, IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository)
        {
            _userManager = userManager;
            this.orderRepository = orderRepository;
            this.orderDetailRepository = orderDetailRepository;
        }

        [HttpGet]
        [Authorize(Roles = RoleName.USER)]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var orders = orderRepository.GetOrdersByUserIdOrderByUpdateTimeDesc(user.Id);
            return View(orders);
        }

        [HttpGet]
        [Authorize(Roles = RoleName.USER)]
        [Route("Orders/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var order = orderRepository.GetOrderByOrderIdAndUserId(id, user.Id);
            if(order == null)
            {
                TempData["Message"] = "You cannot access this order";
                TempData["IsSuccess"] = "false";
                return RedirectToAction("Index");
            }
            var orderDetails = orderDetailRepository.GetByOrderIdIncludeProduct(order.Id);
            return View(orderDetails);
        }
    }
}
