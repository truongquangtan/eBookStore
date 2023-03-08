using BookStoreWebApp.Models;
using BookStoreWebApp.Models.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Repositories.Repositories.CartRepository;
using Repositories.Repositories.UserRepository;
using Repositories.Repositories.ProductRepository;
using Repositories.Repositories.OrderRepository;
using Repositories.Repositories.OrderDetailRepository;

namespace BookStoreWebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ICartRepository cartRepository;
        private readonly IUserRepository userRepository;
        private readonly IProductRepository productRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IOrderDetailRepository orderDetailRepository;

        public CartController(
            UserManager<User> userManager,
            ICartRepository cartRepository,
            IUserRepository userRepository,
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository
           )
        {
            _userManager = userManager;
            this.cartRepository = cartRepository;
            this.userRepository = userRepository;
            this.productRepository = productRepository;
            this.orderRepository = orderRepository;
            this.orderDetailRepository = orderDetailRepository;
        }

        public async Task<IActionResult> Index()
        {
            if(HttpContext.User.Identity.Name == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var cartItem = cartRepository.GetCartByUserId(user.Id);
            return View(cartItem);
        }
        [HttpPost]
        [Route("add-to-cart")]
        public bool AddToCart(AddToCardRequest request)
        {
            var user = userRepository.GetUserByUsername(HttpContext.User.Identity.Name);
            //Find old cart item
            var cartItem = cartRepository.GetCartByProductIdAndUserId(request.ProductId, user.Id);

            if(cartItem != null)
            {
                cartItem.Quantity++;
                cartRepository.Update(cartItem);
                return true;
            }

            cartRepository.Add(new Cart()
            {
                ProductId = request.ProductId,
                UserId = user?.Id,
                Quantity = 1,
            });
            return true;
        }

        [HttpGet]
        [Route("Cart/DeleteItem/{id}")]
        public IActionResult DeleteItem(string id)
        {
            var user = userRepository.GetUserByUsername(HttpContext.User.Identity.Name);
            var cartItem = cartRepository.GetById(id);
            if(cartItem != null && cartItem.UserId == user.Id)
            {
                cartRepository.Remove(cartItem);
                TempData["Message"] = "Delete item from cart successfully";
                TempData["IsSuccess"] = "true";
            }
            else
            {
                TempData["Message"] = "Cannot delete this item";
                TempData["IsSuccess"] = "false";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("Cart/TotalItems")]
        public string GetTotalItems(GetTotalItemsRequest request)
        {
            if (string.IsNullOrEmpty(request.Username))
            {
                return "0";
            }
            var user = userRepository.GetUserByUsername(HttpContext.User.Identity.Name);
            var total = cartRepository.GetCartCountByUserId(user.Id);
            return total.ToString();
        }

        [HttpPost]
        [Route("Cart/UpdateCart")]
        public IActionResult UpdateCart(UpdateCartItem request)
        {
            var cartItem = cartRepository.GetById(request.Id);
            if(cartItem == null)
            {
                return RedirectToAction("Index");
            }
            if(int.Parse(request.Quantity) == 0)
            {
                cartRepository.Remove(cartItem);
                TempData["Message"] = "Update cart successfully";
                TempData["IsSuccess"] = "true";
                return RedirectToAction("Index");
            }
            cartItem.Quantity = int.Parse(request.Quantity);
            cartRepository.Update(cartItem);
            TempData["Message"] = "Update cart successfully";
            TempData["IsSuccess"] = "true";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("Cart/Checkout")]
        public async Task<IActionResult> Checkout(CheckoutRequest request)
        {
            // Validation
            if(string.IsNullOrEmpty(request.Address))
            {
                TempData["AddressError"] = "Request cannot be empty";
                return RedirectToAction("Index");
            }
            if (Regex.IsMatch(request.Phone, "[0-9]{10}"))
            {
                //
            }
            else
            {
                TempData["PhoneError"] = "Phone is not in format";
                return RedirectToAction("Index");
            }

            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var cart = cartRepository.GetCartProductByUserId(user.Id);

            if(cart.Count <= 0 || cart[0].UserId != user.Id)
            {
                return Forbid();
            }

            // CHECKOUT PROCESS

            // Calculate total price

            long total = 0;
            foreach(var item in cart)
            {
                // Check Product quantity
                var product = productRepository.GetById(item.ProductId);
                if(product.Quantity < item.Quantity)
                {
                    TempData["Message"] = $"The book name {item.Product.Name} with quantity {item.Quantity} is not available. Try to select with less amount or delete it from cart";
                    TempData["IsSuccess"] = "false";
                    return RedirectToAction("Index");
                }
                total += item.Quantity.Value * (long) item.Product.Price.Value;
            }

            foreach(var item in cart)
            {
                // Decrease the product quantity
                var product = productRepository.GetById(item.ProductId);
                product.Quantity -= item.Quantity;
                productRepository.Update(product);
            }

            // Create order summary
            var order = new OrderSum()
            {
                Method = "DEFAULT",
                DeliveryAddress = request.Address,
                Total = total,
                UserId = user.Id,
                Phone = request.Phone,
            };
            orderRepository.Add(order);

            // Create order detail
            List<OrderDetail> orderDetails = new();
            foreach (var item in cart)
            {
                var orderDetail = new OrderDetail()
                {
                    OrderId = order.Id,
                    Price = item.Quantity * (long)item.Product.Price,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId,
                };
                orderDetails.Add(orderDetail);
            }
            orderDetailRepository.AddRange(orderDetails);

            // Remove the cart
            cartRepository.RemoveRangeCart(cart);

            //Show message
            TempData["Message"] = "You've checkout successfully, choose order history to see.";
            TempData["IsSuccess"] = "true";

            return RedirectToAction("Index", "Home");
        }
    }
}
