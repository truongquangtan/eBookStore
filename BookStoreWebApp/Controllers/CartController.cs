﻿using BookStoreWebApp.Models;
using BookStoreWebApp.Models.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BookStoreWebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly eBookStore5Context context;
        private readonly UserManager<User> _userManager;

        public CartController(eBookStore5Context context, UserManager<User> userManager)
        {
            this.context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if(HttpContext.User.Identity.Name == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var cartItem = context.Carts.Where(c => c.UserId == user.Id).Include(c => c.Product).Include(c => c.Product.ProductImages).ToList();
            return View(cartItem);
        }
        [HttpPost]
        [Route("add-to-cart")]
        public async Task<bool> AddToCart(AddToCardRequest request)
        {
            var user = context.Users.Where(user => user.Mail == request.Username).FirstOrDefault();
            //Find old cart item
            var cartItem = context.Carts.Where(c => c.ProductId == request.ProductId && c.UserId == user.Id).FirstOrDefault();

            if(cartItem != null)
            {
                cartItem.Quantity++;
                context.Carts.Update(cartItem);
                await context.SaveChangesAsync();
                return true;
            }

            context.Carts.Add(new Cart()
            {
                ProductId = request.ProductId,
                UserId = user?.Id,
                Quantity = 1,
            });
            await context.SaveChangesAsync();
            return true;
        }

        [HttpGet]
        [Route("Cart/DeleteItem/{id}")]
        public IActionResult DeleteItem(string id)
        {
            var user = context.Users.Where(u => u.Mail == HttpContext.User.Identity.Name).FirstOrDefault();
            var cartItem = context.Carts.Where(c => c.Id == id).FirstOrDefault();
            if(cartItem != null && cartItem.UserId == user.Id)
            {
                context.Carts.Remove(cartItem);
                context.SaveChanges();
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
            var user = context.Users.Where(user => user.Mail == request.Username).FirstOrDefault();
            var total = context.Carts.Where(c => c.UserId == user.Id).Count();
            return total.ToString();
        }

        [HttpPost]
        [Route("Cart/UpdateCart")]
        public IActionResult UpdateCart(UpdateCartItem request)
        {
            var cartItem = context.Carts.Where(c => c.Id == request.Id).FirstOrDefault();
            if(cartItem == null)
            {
                return RedirectToAction("Index");
            }
            if(int.Parse(request.Quantity) == 0)
            {
                context.Carts.Remove(cartItem);
                context.SaveChanges();
                TempData["Message"] = "Update cart successfully";
                TempData["IsSuccess"] = "true";
                return RedirectToAction("Index");
            }
            cartItem.Quantity = int.Parse(request.Quantity);
            context.Carts.Update(cartItem);
            context.SaveChanges();
            TempData["Message"] = "Update cart successfully";
            TempData["IsSuccess"] = "true";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("Cart/Checkout")]
        public async Task<IActionResult> Checkout(CheckoutRequest request)
        {
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var cart = await context.Carts.Where(c => c.UserId == user.Id).Include(c => c.Product).ToArrayAsync();

            if(cart.Length <= 0 || cart[0].UserId != user.Id)
            {
                return Forbid();
            }

            long total = 0;
            foreach(var item in cart)
            {
                total += item.Quantity.Value * (long) item.Product.Price.Value;
            }

            var order = new OrderSum()
            {
                Method = "DEFAULT",
                DeliveryAddress = request.Address,
                Total = total,
                UserId = user.Id,
                Phone = request.Phone,
            };
            context.Orders.Add(order);
            await context.SaveChangesAsync();

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
            await context.OrderDetails.AddRangeAsync(orderDetails);
            await context.SaveChangesAsync();

            context.Carts.RemoveRange(cart);
            await context.SaveChangesAsync();

            TempData["Message"] = "You've checkout successfully, choose order history to see.";
            TempData["IsSuccess"] = "true";

            return RedirectToAction("Index", "Home");
        }
    }
}