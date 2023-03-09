using BookStoreWebApp.Models;
using BookStoreWebApp.Models.Request;
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
    [Authorize(Roles = RoleName.ADMIN)]
    public class OrderAdminController : Controller
    {
        private readonly IOrderRepository orderRepository;
        private readonly IOrderDetailRepository orderDetailRepository;

        public OrderAdminController(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository)
        {
            this.orderRepository = orderRepository;
            this.orderDetailRepository = orderDetailRepository;
        }

        [HttpGet]
        public IActionResult Index(string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                var allOrders = orderRepository.GetAllOrdersByCreatedTime();
                return View(allOrders);
            }
            var orders = orderRepository.GetAllOrdersFilterByStatusByCreatedTime(status);
            TempData["Status"] = status;
            return View(orders);
        }

        [HttpGet]
        [Route("Admin/Orders/{id}")]
        public IActionResult Details(int id)
        {
            var order = orderRepository.GetById(id);
            if(order == null)
            {
                return NotFound();
            }
            var orderDetails = orderDetailRepository.GetByOrderIdIncludeProduct(id);
            order.OrderDetails = orderDetails.ToList();
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(ChangeOrderStatusRequest request)
        {
            if(request.NewStatus != OrderStatus.CREATED
                && request.NewStatus != OrderStatus.ACCEPTED
                && request.NewStatus != OrderStatus.SHIPPED)
            {
                return BadRequest();
            }
            var order = orderRepository.GetById(request.OrderId);
            if(order == null)
            {
                return NotFound();
            }

            order.Status = request.NewStatus;
            orderRepository.Update(order);
            TempData["Message"] = "You've change status of a order successfully.";
            TempData["IsSuccess"] = "true";
            return RedirectToAction("Details", new { id = request.OrderId });
        }
    }
}
